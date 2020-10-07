using System.Collections.Generic;

using Contador.Api.Models;
using Contador.Core.Common;

namespace Contador.Api.Services
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
        Result<ResponseCode, ExpenseCategory> GetCategoryById(int id);

        /// <summary>
        /// Gets all available expense categories.
        /// </summary>
        /// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
        Result<ResponseCode, IList<ExpenseCategory>> GetCategories();

        /// <summary>
        /// Adds expense category.
        /// </summary>
        /// <param name="category">Expense category to add.</param>
        /// <returns><see cref="ResponseCode"/> for operation and added category.</returns>
        Result<ResponseCode, ExpenseCategory> Add(ExpenseCategory category);

        /// <summary>
        /// Updates expense category of provided id.
        /// </summary>
        /// <param name="id">Id of category to update.</param>
        /// <param name="category">Category info.</param>
        /// <returns>Updated category</returns>
        Result<ResponseCode, ExpenseCategory> Update(int id, ExpenseCategory category);

        /// <summary>
        /// Removes expense category of provided id.
        /// </summary>
        /// <param name="id">Id of category to remove.</param>
        /// <returns><see cref="ResponseCode"/> of operation.</returns>
        ResponseCode Remove(int id);
    }
}
