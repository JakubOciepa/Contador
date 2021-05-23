using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Services.Interfaces;

namespace Contador.Services
{
	/// <summary>
	/// Provides mechanism to manage Contador issues.
	/// </summary>
	public class IssueManager : IIssueManager
	{
		/// <summary>
		/// Invoked when new(returned) issue has been added.
		/// </summary>
		public event EventHandler<Issue> IssueAdded;

		/// <summary>
		/// Add the Contador issue.
		/// </summary>
		/// <param name="issue">Information about issue to add.</param>
		/// <returns>Added issue.</returns>
		public async Task<Result<Issue>> AddIssueAsync(Issue issue)
		{
			IssueAdded?.Invoke(this, issue);
			return await Task.FromResult(new Result<Issue>(ResponseCode.Ok ,issue));
		}

		/// <summary>
		/// Gets all created and opened issues.
		/// </summary>
		/// <returns></returns>
		public async Task<Result<IEnumerable<Issue>>> GetAllIssuesAsync()
		{
			return await Task.FromResult(new Result<IEnumerable<Issue>>(ResponseCode.Ok, new List<Issue>()));
		}
	}
}
