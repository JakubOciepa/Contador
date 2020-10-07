using System.Collections.Generic;

using Contador.Core.Models;
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
        private readonly List<ExpenseCategory> _stub;

        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryRepository"/> class.
        /// </summary>
        /// <param name="context">DbContext.</param>
        public ExpenseCategoryRepository()
        {
            //_db = context;
            _stub = new List<ExpenseCategory>
            {
                new ExpenseCategory()
                {
                    Id = 0,
                    Name = "Słodycze",
                },
            };
        }

        ///<inheritdoc/>
        public ExpenseCategory GetCategoryById(int categoryId)
        {
            return _stub.Find(c => c.Id == categoryId);
        }
    }
}
