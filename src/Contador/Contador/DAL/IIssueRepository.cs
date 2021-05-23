using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.DAL
{
	// <summary>
	/// Provides mechanism to manage Contador issues.
	/// </summary>
	public interface IIssueRepository
	{
		/// <summary>
		/// Gets all created and opened issues.
		/// </summary>
		/// <returns></returns>
		Task<Result<IEnumerable<Issue>>> GetAllIssuesAsync();

		/// <summary>
		/// Add the Contador issue.
		/// </summary>
		/// <param name="issue">Information about issue to add.</param>
		/// <returns>Added issue.</returns>
		Task<Result<Issue>> AddIssueAsync(Issue issue);
	}
}
