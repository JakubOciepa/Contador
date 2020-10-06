using Contador.Core.Models;

namespace Contador.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Manages expense categories in db.
    /// </summary>
    public interface IExpenseCategoryRepository
    {
        /// <summary>
        /// Gets category by its id.
        /// </summary>
        /// <param name="categoryId">Id of requested <see cref="ExpenseCategory"/>.</param>
        /// <returns><see cref="ExpenseCategory"/> of requested Id.</returns>
        ExpenseCategory GetCategoryById(int categoryId);
    }
}