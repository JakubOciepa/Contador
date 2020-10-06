﻿using Contador.Core.Models;
using Contador.DAL.DbContext;
using Contador.DAL.Repositories.Interfaces;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expense categories in db.
    /// </summary>
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly ContadorContext _db;

        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryRepository"/> class.
        /// </summary>
        /// <param name="context">DbContext.</param>
        public ExpenseCategoryRepository()
        {
            //_db = context;
        }

        /// <summary>
        /// Gets category by its id.
        /// </summary>
        /// <param name="categoryId">Id of requested <see cref="ExpenseCategory"/>.</param>
        /// <returns><see cref="ExpenseCategory"/> of requested Id.</returns>
        public ExpenseCategory GetCategoryById(int categoryId)
        {
            return new ExpenseCategory() { Name = "Słodycze" };
        }
    }
}
