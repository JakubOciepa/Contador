using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Abstractions
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
		Task<Result<ExpenseCategory>> GetCategoryByIdAsync(int id);

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
		Task<Result<IList<ExpenseCategory>>> GetCategoriesAsync();

		/// <summary>
		/// Adds expense category.
		/// </summary>
		/// <param name="category">Expense category to add.</param>
		/// <returns><see cref="ResponseCode"/> for operation and added category.</returns>
		Task<Result<ExpenseCategory>> AddExpenseAsync(ExpenseCategory category);

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to update.</param>
		/// <param name="category">Category info.</param>
		/// <returns>Updated category</returns>
		Task<Result<ExpenseCategory>> UpdateExpenseAsync(int id, ExpenseCategory category);

		/// <summary>
		/// Removes expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to remove.</param>
		/// <returns><see cref="ResponseCode"/> of operation.</returns>
		Task<ResponseCode> RemoveExpenseAsync(int id);
	}
}
