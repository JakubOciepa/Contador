using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Services.Interfaces
{
	public interface IBudgetService
	{
		/// <summary>
		/// Gets <see cref="Budget"/> by id.
		/// </summary>
		/// <param name="id">Id of requested budget.</param>
		/// <returns>Correct budget or default</returns>
		Task<Result<Budget>> GetByIdAsync(int id);

		/// <summary>
		/// Adds the budget to the repository.
		/// </summary>
		/// <param name="budget">Budget to add.</param>
		/// <returns>Added budget.</returns>
		Task<Result<Budget>> AddAsync(Budget budget);
	}
}
