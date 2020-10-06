using Contador.Core.Models;
using Contador.DAL.DbContext;
using System.Collections.Generic;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expenses in db.
    /// </summary>
    public class ExpensesRepository
    {
        private readonly ContadorContext _db;

        public ExpensesRepository(ContadorContext context)
        {
            _db = context;
        }

        public IList<Expense> GetExpenses()
        {
            
            return new[]
            {
                new Expense("Marysia", 123, 0,0),
                new Expense("Marysia", 123, 0,0),
                new Expense("Marysia", 123, 0,0),
            };
        }

        public Expense GetExpense(int expenseId)
        {
            return new Expense("Marysia", 123,0,0);
        }
    }
}
