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
            const string procedure = "expense_GetById";
            var expense = (await _dbConnection
                .QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(procedure,
                    (expense, category, user) =>
                    {
                        expense.Category = category;
                        expense.User = user;

                        return expense;
                    },
                    new { expenseId },
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
            const string procedure = "expense_GetAll";

            var expenses = await _dbConnection
                .QueryAsync<ExpenseDto, ExpenseCategoryDto, UserDto, ExpenseDto>(procedure,
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
            const string procedure = "expense_Add";

            var param = new DynamicParameters();
            param.Add("name_p", expense.Name);
            param.Add("value_p", expense.Value);
            param.Add("description_p", expense.Description);
            param.Add("categoryId_p", expense.Category.Id);
            param.Add("userId_p", expense.User.Id);
            param.Add("image_path_p", string.Empty);

            await _dbConnection.ExecuteAsync(procedure, param, commandType: CommandType.StoredProcedure).CAF();

            return expense;
        }

        /// <inheritdoc/>
        public async Task<Expense> Update(int id, Expense expense)
        {
            const string procedure = "expense_Update";

            var param = new DynamicParameters();
            param.Add("id_p", expense.Id);
            param.Add("name_p", expense.Name);
            param.Add("value_p", expense.Value);
            param.Add("description_p", expense.Description);
            param.Add("categoryId_p", expense.Category.Id);
            param.Add("userId_p", expense.User.Id);
            param.Add("image_path_p", string.Empty);

            await _dbConnection.ExecuteAsync(procedure, param, commandType: CommandType.StoredProcedure).CAF();

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