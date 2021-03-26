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
		/// Adds provided <see cref="Expense"/> to storage.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>Added expense or default</returns>
		Task<Expense> AddExpenseAsync(Expense expense);

		/// <summary>
		/// Updates <see cref="Expense"/> of provided id.
		/// </summary>
		/// <param name="id">Id of expense to update.</param>
		/// <param name="info">Expense info.</param>
		/// <returns>Updated expense or default.</returns>
		Task<Expense> UpdateExpenseAsync(int id, Expense info);

		/// <summary>
		/// Removes <see cref="Expense"/> of provided id from storage.
		/// </summary>
		/// <param name="id">Id of expense to remove.</param>
		/// <returns>True if removed, false otherwise.</returns>
		Task<bool> RemoveExpenseAsync(int id);
	}
}
