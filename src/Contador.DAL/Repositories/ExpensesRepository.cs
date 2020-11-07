using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.Models;

using Dapper;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expenses in db.
    /// </summary>
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly IDbConnection _dbConnection;

        private static readonly List<Expense> _stub = new List<Expense>
            {
                new Expense("Słodycze",0,null,null){ Id = 0 },
                new Expense("Słodycze",0,null,null){ Id = 1 },
                new Expense("Słodycze",0,null,null){ Id = 2 },
            };

        /// <summary>
        /// Creates instance of <see cref="ExpensesRepository"/> class.
        /// </summary>
        /// <param name="dbConnection">Connection to database.</param>
        public ExpensesRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        ///<inheritdoc/>
        public async Task<Expense> GetExpense(int expenseId)
        {
            var parameter = new DynamicParameters();
            parameter.Add(ExpenseDto.ParameterName.Id, expenseId);

            var expense = (await _dbConnection
                .QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetById,
                    (expense, category, user) =>
                    {
                        expense.Category = category;
                        expense.User = user;

                        return expense;
                    },
                    parameter,
                    commandType: CommandType.StoredProcedure)
                .CAF()).FirstOrDefault();

            if (expense is object)
            {
                return new Expense(expense.Name, expense.Value, expense.User, expense.Category)
                {
                    Id = expense.Id,
                    Description = expense.Description,
                };
            }

            return default;
        }

        ///<inheritdoc/>
        public async Task<IList<Expense>> GetExpenses()
        {
            var expenses = await _dbConnection
                .QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(ExpenseDto.ProcedureName.GetAll,
                    (expense, category, user) =>
                    {
                        expense.Category = category;
                        expense.User = user;

                        return expense;
                    },
                    commandType: CommandType.StoredProcedure)
                .CAF();

            return expenses.Cast<Expense>().ToList();
        }

        /// <inheritdoc/>
        public async Task<Expense> Add(Expense expense)
        {
            var param = new DynamicParameters();
            param.Add(ExpenseDto.ParameterName.Name, expense.Name);
            param.Add(ExpenseDto.ParameterName.Value, expense.Value);
            param.Add(ExpenseDto.ParameterName.Description, expense.Description);
            param.Add(ExpenseDto.ParameterName.CategoryId, expense.Category.Id);
            param.Add(ExpenseDto.ParameterName.UserId, expense.User.Id);
            param.Add(ExpenseDto.ParameterName.ImagePath, string.Empty);

            await _dbConnection.ExecuteAsync(ExpenseDto.ProcedureName.Add, param, commandType: CommandType.StoredProcedure).CAF();

            return expense;
        }

        /// <inheritdoc/>
        public async Task<Expense> Update(int id, Expense expense)
        {
            var param = new DynamicParameters();
            param.Add(ExpenseDto.ParameterName.Id, expense.Id);
            param.Add(ExpenseDto.ParameterName.Name, expense.Name);
            param.Add(ExpenseDto.ParameterName.Value, expense.Value);
            param.Add(ExpenseDto.ParameterName.Description, expense.Description);
            param.Add(ExpenseDto.ParameterName.CategoryId, expense.Category.Id);
            param.Add(ExpenseDto.ParameterName.UserId, expense.User.Id);
            param.Add(ExpenseDto.ParameterName.ImagePath, string.Empty);

            await _dbConnection.ExecuteAsync(ExpenseDto.ProcedureName.Update, param, commandType: CommandType.StoredProcedure).CAF();

            return expense;
        }

        /// <inheritdoc/>
        public async Task<bool> Remove(int id)
        {
            var expenseToRemove = _stub.Find(e => e.Id == id);

            if (expenseToRemove == default)
            {
                return true;
            }

            _stub.Remove(expenseToRemove);

            return !_stub.Contains(expenseToRemove);
        }
    }
}