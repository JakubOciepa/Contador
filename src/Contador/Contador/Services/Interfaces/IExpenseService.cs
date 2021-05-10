using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Abstractions
{
	/// <summary>
	/// Expense manager.
	/// </summary>
	public interface IExpenseService
	{
		/// <summary>
		/// Gets all available expenses.
		/// </summary>
		/// <returns><see cref="Result{IList{Expense}}"/> with the proper <see cref="ResponseCode"/> and the <see cref="IList{Expense}"/>.</returns>
		Task<Result<IList<Expense>>> GetAllAsync();

		/// <summary>
		/// Gets the <see cref="Expense"/> of the provided id.
		/// </summary>
		/// <param name="id">Id of the requested <see cref="Expense"/>.</param>
		/// <returns><see cref="Expense"/> of provided id.</returns>
		Task<Result<Expense>> GetByIdAsync(int id);

		/// <summary>
		/// Gets the <see cref="Expense"/> for the provided month.
		/// </summary>
		/// <param name="month">Month of the expenses creation.</param>
		/// <param name="year">Year of the expenses creation.</param>
		/// <returns><see cref="IList{Expense}"/> which were created in provided month.</returns>
		Task<Result<IList<Expense>>> GetByMonthAsync(int month, int year);

		/// <summary>
		/// Gets the <see cref="Expense"/> for the provided year.
		/// </summary>
		/// <param name="year">Year of the expenses creation.</param>
		/// <returns><see cref="IList{Expense}"/> which were created in provided year.</returns>
		Task<Result<IList<Expense>>> GetByYearAsync(int year);

		/// <summary>
		/// Gets provided count or less of latest expenses.
		/// </summary>
		/// <param name="count">Amount of expenses to return.</param>
		/// <returns>Provided count or less of the latest </returns>
		Task<Result<IList<Expense>>> GetLatest(int count);

		/// <summary>
		/// Gets expenses filtered by provided values.
		/// </summary>
		/// <param name="name">Name of the expense of part of the name to filter.</param>
		/// <param name="categoryName">Name of the category to filter.</param>
		/// <param name="userName">Name of the user to filter.</param>
		/// <param name="createDate">Create date of the expense</param>
		/// <returns>List of the expenses that fulfill the requirements</returns>
		Task<Result<IList<Expense>>> GetFiltered(string name, string categoryName, string userName, DateTime createDate);

		/// <summary>
		/// Adds the provided <see cref="Expense"/> into the storage.
		/// </summary>
		/// <param name="expense">Expense to add.</param>
		/// <returns>Correct <see cref="ResponseCode"/> for operation and added expense.</returns>
		Task<Result<Expense>> AddAsync(Expense expense);

		/// <summary>
		/// Updates the <see cref="Expense"/> of the provided id.
		/// </summary>
		/// <param name="id">Id of the expense to update.</param>
		/// <param name="expense">Expense info.</param>
		/// <returns>Correct <see cref="ResponseCode"/> for operation and updated expense.</returns>
		Task<Result<Expense>> UpdateAsync(int id, Expense expense);

		/// <summary>
		/// Removes the <see cref="Expense"/> of the provided id.
		/// </summary>
		/// <param name="id">Id of the expense to remove.</param>
		/// <returns>Correct <see cref="ResponseCode"/> for operation</returns>
		Task<ResponseCode> RemoveAsync(int id);
	}
}
