using Contador.Core.Models;

namespace Contador.Api.Services.Interfaces
{
    /// <summary>
    /// Expense categories manager.
    /// </summary>
    public interface IExpenseCategoryService
    {
        /// <summary>
        /// Gets <see cref="ExpenseCategory"/> by id.
        /// </summary>
        /// <param name="id">Id of requested expense category</param>
        /// <returns>Correct ExpenseCategory or default</returns>
        ExpenseCategory GetCategoryById(int id);
    }
}
