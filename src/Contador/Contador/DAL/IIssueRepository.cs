using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Models;

namespace Contador.DAL
{
	// <summary>
	/// Provides mechanism to manage Contador issues.
	/// </summary>
	public interface IIssueRepository
	{
		/// <summary>
		/// Gets all created issues.
		/// </summary>
		/// <returns>List of all available issues.</returns>
		Task<IEnumerable<Issue>> GetAllAsync();

		/// <summary>
		/// Add the Contador issue.
		/// </summary>
		/// <param name="issue">Information about issue to add.</param>
		/// <returns>Added issue.</returns>
		Task<Issue> AddAsync(Issue issue);
	}
}
