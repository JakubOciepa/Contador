﻿using Contador.Core.Models;
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

        /// <summary>
        /// Creates instance of <see cref="ExpensesRepository"/> class.
        /// </summary>
        /// <param name="context">DbContext.</param>
        public ExpensesRepository(ContadorContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Gets all available expenses.
        /// </summary>
        /// <returns><see cref="IList{Expense}"/> of all available expenses.</returns>
        public IList<Expense> GetExpenses()
        {
            return new[]
            {
                new Expense("Marysia", 123, 0,0),
                new Expense("Marysia", 123, 0,0),
                new Expense("Marysia", 123, 0,0),
            };
        }

        /// <summary>
        /// Gets <see cref="Expense"/> of provided id.
        /// </summary>
        /// <param name="expenseId">Id of requested <see cref="Expense"/>.</param>
        /// <returns><see cref="Expense"/> of provided Id.</returns>
        public Expense GetExpense(int expenseId)
        {
            return new Expense("Marysia", 123, 0, 0);
        }
    }
}
