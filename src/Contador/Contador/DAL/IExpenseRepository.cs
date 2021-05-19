using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;

namespace Contador.DAL.Abstractions
{
	/// <summary>
	/// Manages expenses in db.
	/// </summary>
	public interface IExpenseRepository
	{
		/// <summary>
		/// Gets <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="expenseId">Id of requested <see cref="Expense"/>.</param>
		/// <returns><see cref="Expense"/> of provided Id.</returns>
		Task<Expense> GetByIdAsync(int expenseId);

		/// <summary>
		/// Gets all available expenses.
		/// </summary>
		/// <returns><see cref="IList{Expense}"/> of all available expenses.</returns>
		Task<IList<Expense>> GetAllAsync();

		/// <summary>
		/// Gets all expenses by provided month.
		/// </summary>
		/// <param name="month">Creation month of the expenses.</param>
		/// <param name="year">Creation year of the expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of all expenses from provided month.</returns>
		Task<IList<Expense>> GetByMonthAsync(int month, int year);

		/// <summary>
		/// Gets all expenses by provided year.
		/// </summary>
		/// <param name="year">Creation year of the expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of all expenses from provided year.</returns>
		Task<IList<Expense>> GetByYearAsync(int year);

		/// <summary>
		/// Gets the count or less of latest expenses.
		/// </summary>
		/// <param name="count">Max count of latest expenses to return.</param>
		/// <returns>The count or less of latest expenses.</returns>
		Task<IList<Expense>> GetLatest(int count);

		/// <summary>
		/// Gets expenses filtered by provided values.
		/// </summary>
		/// <param name="name">Name of the expense of part of the name to filter.</param>
		/// <param name="categoryName">Name of the category to filter.</param>
		/// <param name="userName">Name of the user to filter.</param>
		/// <returns>List of the expenses that fulfill the requirements</returns>
		Task<IList<Expense>> GetFiltered(string name, string categoryName, string userName);

		/// <summary>
		/// Gets all expenses for provided category.
		/// </summary>
		/// <param name="categoryId">Category id of searched expenses.</param>
		/// <returns><see cref="IList{Expense}"/> of expenses in this category.</returns>
		Task<IList<Expense>> GetByCategory(int categoryId);


		/// <summary>
		/// Adds provided <see cref="Expense"/> to the storage.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>The added expense.</returns>
		Task<Expense> AddAsync(Expense expense);

		/// <summary>
		/// Updates the <see cref="Expense"/> of the provided id.
		/// </summary>
		/// <param name="id">Id of the expense to update.</param>
		/// <param name="info">Expense info.</param>
		/// <returns>Updated expense.</returns>
		Task<Expense> UpdateAsync(int id, Expense info);

		/// <summary>
		/// Removes the <see cref="Expense"/> of the provided id from the storage.
		/// </summary>
		/// <param name="id">Id of the expense to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		Task<bool> RemoveAsync(int id);
	}
}
