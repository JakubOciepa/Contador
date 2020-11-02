using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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
                new Expense("Słodkości", 123, 0,0){ Id = 0 },
                new Expense("Słodkości", 123, 0,0){ Id = 1 },
                new Expense("Słodkości", 123, 0,0){ Id = 2 },
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
            return _stub.Find(e => e.Id == expenseId);
        }

        ///<inheritdoc/>
        public async Task<IList<Expense>> GetExpenses()
        {
            var expenses = _dbConnection.Query<Expense>("SELECT * FROM Expenses");

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
            expenseToUpdate.CategoryId = info.CategoryId;
            expenseToUpdate.UserId = info.UserId;
            expenseToUpdate.Description = info.Description;
            expenseToUpdate.ModifiedDate = DateTime.Now;

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
