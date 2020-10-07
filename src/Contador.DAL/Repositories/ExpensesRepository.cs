using Contador.Core.Models;
using Contador.DAL.DbContext;
using Contador.DAL.Repositories.Interfaces;
using System.Collections.Generic;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expenses in db.
    /// </summary>
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly ContadorContext _db;
        private readonly List<Expense> _stub;

        /// <summary>
        /// Creates instance of <see cref="ExpensesRepository"/> class.
        /// </summary>
        /// <param name="context">DbContext.</param>
        public ExpensesRepository()
        {
            //_db = context;
            _stub = new List<Expense>
            {
                new Expense("Słodkości", 123, 0,0){ Id=0 },
                new Expense("Słodkości", 123, 0,0){ Id=1 },
                new Expense("Słodkości", 123, 0,0){ Id=2 },
            };
        }

        ///<inheritdoc/>
        public Expense GetExpense(int expenseId)
        {
            return _stub.Find(e => e.Id == expenseId);
        }

        ///<inheritdoc/>
        public IList<Expense> GetExpenses()
        {
            return _stub;
        }
    }
}
