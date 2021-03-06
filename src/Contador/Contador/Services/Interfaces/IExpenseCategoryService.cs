﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Services.Interfaces
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
		Task<Result<ExpenseCategory>> GetByIdAsync(int id);

		/// <summary>
		/// Gets all available expense categories.
		/// </summary>
		/// <returns><see cref="IList{ExpenseCategory}"/> of all available categories.</returns>
		Task<Result<IList<ExpenseCategory>>> GetAllAsync();

		/// <summary>
		/// Adds expense category.
		/// </summary>
		/// <param name="category">Expense category to add.</param>
		/// <returns><see cref="ResponseCode"/> for operation and added category.</returns>
		Task<Result<ExpenseCategory>> AddAsync(ExpenseCategory category);

		/// <summary>
		/// Updates expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to update.</param>
		/// <param name="category">Category info.</param>
		/// <returns>Updated category</returns>
		Task<Result<ExpenseCategory>> UpdateAsync(int id, ExpenseCategory category);

		/// <summary>
		/// Removes expense category of provided id.
		/// </summary>
		/// <param name="id">Id of category to remove.</param>
		/// <returns><see cref="ResponseCode"/> of operation.</returns>
		Task<ResponseCode> RemoveAsync(int id);
	}
}
