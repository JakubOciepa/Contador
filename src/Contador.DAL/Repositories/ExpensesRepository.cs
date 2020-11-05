using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Utils.Extensions;
using Contador.Core.Models;

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
            return await _dbConnection
                .QueryFirstOrDefaultAsync<Expense>($"SELECT * FROM Expense WHERE Id = {expenseId}").CAF();
        }

        ///<inheritdoc/>
        public async Task<IList<Expense>> GetExpenses()
        {
            var expenses = await _dbConnection.QueryAsync<Expense>("SELECT * FROM Expense").CAF();

            return expenses.ToList();
        }

        /// <inheritdoc/>
        public async Task<Expense> Add(Expense expense)
        {
            var lastId = _stub.Max(e => e.Id);
            expense.Id = lastId + 1;
            _stub.Add(expense);

            return expense;
        }

        /// <inheritdoc/>
        public async Task<Expense> Update(int id, Expense info)
        {
            var expenseToUpdate = _stub.Find(e => e.Id == id);

            if (expenseToUpdate == default)
            {
                return default;
            }

            expenseToUpdate.Name = info.Name;
            expenseToUpdate.Value = info.Value;
            expenseToUpdate.Category = info.Category;
            expenseToUpdate.User = info.User;
            expenseToUpdate.Description = info.Description;

            return expenseToUpdate;
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
