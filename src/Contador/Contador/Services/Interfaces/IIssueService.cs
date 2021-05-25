using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.Services.Interfaces
{
	/// <summary>
	/// Provides mechanism to manage Contador issues.
	/// </summary>
	public interface IIssueService
	{
		/// <summary>
		/// Gets all created issues.
		/// </summary>
		/// <returns>List of all available issues and correct code.</returns>
		Task<Result<IList<Issue>>> GetAllAsync();

		/// <summary>
		/// Add the Contador issue.
		/// </summary>
		/// <param name="issue">Information about issue to add.</param>
		/// <returns>Added issue.</returns>
		Task<Result<Issue>> AddAsync(Issue issue);
	}
}
