using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contador.DAL.DbContext;
using Contador.DAL.Models;

namespace Contador.DAL.Repositories
{
    /// <summary>
    /// Manages expense categories in db.
    /// </summary>
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly ContadorContext _db;
        private static readonly List<ExpenseCategory> _stub = new List<ExpenseCategory>
        {
            new ExpenseCategory("Słodycze")
            {
                Id = 0,
            },
        };

        /// <summary>
        /// Creates instance of <see cref="ExpenseCategoryRepository"/> class.
        /// </summary>
        /// <param name="context">DbContext.</param>
        public ExpenseCategoryRepository()
        {
            //_db = context;
        }

        /// <inheritdoc/>
        public ExpenseCategory Add(ExpenseCategory expenseCategory)
        {
            var lastId = _stub.Max(e => e.Id);
            expenseCategory.Id = lastId + 1;
            _stub.Add(expenseCategory);

            return expenseCategory;
        }

        /// <inheritdoc/>
        public async Task<IList<ExpenseCategory>> GetCategories()
        {
            return _stub;
        }

        ///<inheritdoc/>
        public ExpenseCategory GetCategoryById(int categoryId)
        {
            return _stub.Find(c => c.Id == categoryId);
        }

        /// <inheritdoc/>
        public bool Remove(int id)
        {
            var categoryToRemove = _stub.Find(e => e.Id == id);

            if (categoryToRemove == default)
            {
                return true;
            }

            _stub.Remove(categoryToRemove);

            return !_stub.Contains(categoryToRemove);
        }

        /// <inheritdoc/>
        public ExpenseCategory Update(int id, ExpenseCategory expenseCategory)
        {
            var categoryToUpdate = _stub.Find(e => e.Id == id);

            if (categoryToUpdate == default)
            {
                return default;
            }

            categoryToUpdate.Name = expenseCategory.Name;

            return categoryToUpdate;
        }
    }
}
