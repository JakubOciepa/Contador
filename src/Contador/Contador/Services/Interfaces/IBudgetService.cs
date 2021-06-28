using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Services.Interfaces
{
	public interface IBudgetService
	{
		/// <summary>
		/// Gets collection of all available budgets.
		/// </summary>
		/// <returns><see cref="List{Budget}"/> of all available budgets or <see langword="null"/></returns>
		Task<Result<List<Budget>>> GetAllBudgetsAsync();

		/// <summary>
		/// Gets <see cref="Budget"/> by id.
		/// </summary>
		/// <param name="id">Id of requested budget.</param>
		/// <returns>Correct budget or <see langword="null"/></returns>
		Task<Result<Budget>> GetBudgetByIdAsync(int id);

		/// <summary>
		/// Adds the budget to the repository.
		/// </summary>
		/// <param name="budget">Budget to add.</param>
		/// <returns>Added budget if added, otherwise <see langword="null"/>.</returns>
		Task<Result<Budget>> AddBudgetAsync(Budget budget);

		/// <summary>
		/// Adds category budget to the repository.
		/// </summary>
		/// <param name="categoryBudget">Category budget to add.</param>
		/// <returns>Added category budget if added, otherwise <see langword="null"/>.</returns>
		Task<Result<CategoryBudget>> AddCategoryBudgetAsync(CategoryBudget categoryBudget);

		/// <summary>
		/// Gets <see cref="CategoryBudget"/> by id.
		/// </summary>
		/// <param name="id">Id of requested budget.</param>
		/// <returns>Correct budget or <see langword="null"/></returns>
		Task<Result<CategoryBudget>> GetCategoryBudgetByIdAsync(int id);

		/// <summary>
		/// Updates budget of provided id.
		/// </summary>
		/// <param name="id">Id of budget to update.</param>
		/// <param name="budget">Budget info.</param>
		/// <returns>Updated budget</returns>
		Task<Result<Budget>> UpdateBudgetAsync(int id, Budget budget);

		/// <summary>
		/// Updates category budget of provided id.
		/// </summary>
		/// <param name="id">Id of category budget to update.</param>
		/// <param name="budget">Category budget info.</param>
		/// <returns>Updated category budget</returns>
		Task<Result<CategoryBudget>> UpdateCategoryBudgetAsync(int id, CategoryBudget budget);

		/// <summary>
		/// Removes the <see cref="Budget"/> of the provided id and its category budgets from the repository.
		/// </summary>
		/// <param name="id">Id of the budget to remove.</param>
		/// <returns><see langword="true"/> if removed, otherwise <see langword="false"/>.</returns>
		Task<ResponseCode> RemoveBudgetAsync(int id);

		/// <summary>
		/// Removes the <see cref="CategoryBudget"/> of the provided id from the repository.
		/// </summary>
		/// <param name="id">Id of the category budget to remove.</param>
		/// <returns><see langword="true"/> if removed, otherwise <see langword="false"/>.</returns>
		Task<ResponseCode> RemoveCategoryBudgetAsync(int id);
	}
}
