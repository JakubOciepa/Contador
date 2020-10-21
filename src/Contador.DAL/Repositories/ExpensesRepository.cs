using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.DAL.DbContext;
using Contador.DAL.Models;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expenses in db.
    /// </summary>
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly ContadorContext _db;
        private static readonly List<Expense> _stub = new List<Expense>
            {
                new Expense("Słodkości", 123, 0,0){ Id = 0 },
                new Expense("Słodkości", 123, 0,0){ Id = 1 },
                new Expense("Słodkości", 123, 0,0){ Id = 2 },
            };

        /// <summary>
        /// Creates instance of <see cref="ExpensesRepository"/> class.
        /// </summary>
        /// <param name="context">DbContext.</param>
        public ExpensesRepository()
        {
            //_db = context;
        }

        ///<inheritdoc/>
        public async Task<Expense> GetExpense(int expenseId)
        {
            return _stub.Find(e => e.Id == expenseId);
        }

        ///<inheritdoc/>
        public async Task<IList<Expense>> GetExpenses()
        {
            return _stub;
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
            expenseToUpdate.LastEditDate = DateTime.Now;

            return expenseToUpdate;
        }

        /// <inheritdoc/>
        public bool Remove(int id)
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
