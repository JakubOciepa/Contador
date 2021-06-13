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
	}
}
