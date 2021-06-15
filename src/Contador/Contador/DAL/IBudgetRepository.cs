using System;
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
		Task<Budget> AddAsync(Budget budgetToAdd);

		/// <summary>
		/// Gets the budget by the provided id.
		/// </summary>
		/// <param name="id">Id of the budget.</param>
		/// <returns>Budget or null if doesn't exists.</returns>
		Task<Budget> GetByIdAsync(int id);

		/// <summary>
		/// Gets the budget by the start date.
		/// </summary>
		/// <param name="startDate">Start date of the budget.</param>
		/// <returns>Budget with the start date.</returns>
		Task<Budget> GetByStartDateAsync(DateTime startDate);
	}
}
