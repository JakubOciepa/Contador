using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;

namespace Contador.DAL.Abstractions
{
	/// <summary>
	/// Manages budgets in the db.
	/// </summary>
	public interface IBudgetRepository
	{
		/// <summary>
		/// Adds the budget to the db.
		/// </summary>
		/// <param name="budgetToAdd">Budget to add.</param>
		/// <returns>Adds budget to the db.</returns>
		Task<Budget> AddBudgetAsync(Budget budgetToAdd);

		/// <summary>
		/// Gets the budget by the provided id.
		/// </summary>
		/// <param name="id">Id of the budget.</param>
		/// <returns>Budget or <see langword="null"/> if doesn't exists.</returns>
		Task<Budget> GetBudgetByIdAsync(int id);

		/// <summary>
		/// Gets the budget by the start date.
		/// </summary>
		/// <param name="startDate">Start date of the budget.</param>
		/// <returns>Budget with the start date.</returns>
		Task<Budget> GetBudgetByStartDateAsync(DateTime startDate);

		/// <summary>
		/// Gets all budgets from the db.
		/// </summary>
		/// <returns>Collection of the available <see cref="Budget"/>.</returns>
		IAsyncEnumerable<Budget> GetAllBudgetsAsync();

		/// <summary>
		/// Add category budget to the db.
		/// </summary>
		/// <param name="categoryBudget">Category budget to add.</param>
		/// <returns>Category budget that has been added.</returns>
		Task<CategoryBudget> AddCategoryBudgetAsync(CategoryBudget categoryBudget);

		/// <summary>
		/// Gets the category budget by the budget id and category id.
		/// </summary>
		/// <param name="budgetId">Budget id.</param>
		/// <param name="categoryId">Category id.</param>
		/// <returns>Category budget if exists, otherwise <see langword="null"/></returns>
		Task<CategoryBudget> GetCategoryBudgetByCategoryAndBudgetIdAsync(int budgetId, int categoryId);

		/// <summary>
		/// Gets the category budget by the provided id.
		/// </summary>
		/// <param name="id">Id of the budget.</param>
		/// <returns>Budget or <see langword="null"/> if doesn't exists.</returns>
		Task<CategoryBudget> GetCategoryBudgetByIdAsync(int id);
	}
}
