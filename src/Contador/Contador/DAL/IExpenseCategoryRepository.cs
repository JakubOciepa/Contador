﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;

namespace Contador.DAL.Abstractions
{
	/// <summary>
	/// Manages expense categories in storage.
	/// </summary>
	public interface IExpenseCategoryRepository
	{
		/// <summary>
		/// Gets category by its id.
		/// </summary>
		/// <param name="categoryId">Id of requested <see cref="ExpenseCategory"/>.</param>
		/// <returns><see cref="ExpenseCategory"/> of requested Id.</returns>
		Task<ExpenseCategory> GetByIdAsync(int categoryId);

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
		Task<IList<ExpenseCategory>> GetAllAsync();

		/// <summary>
		/// Adds expense category to storage.
		/// </summary>
		/// <param name="expenseCategory">Expense category to add.</param>
		/// <returns>Added expense category</returns>
		Task<ExpenseCategory> AddAsync(ExpenseCategory expenseCategory);

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of expense category to update.</param>
		/// <param name="expenseCategory">Category info.</param>
		/// <returns>Updated category.</returns>
		Task<ExpenseCategory> UpdateAsync(int id, ExpenseCategory expenseCategory);

		/// <summary>
		/// Removes expense category from storage.
		/// </summary>
		/// <param name="id">Id of expense category to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		Task<bool> RemoveAsync(int id);
	}
}
