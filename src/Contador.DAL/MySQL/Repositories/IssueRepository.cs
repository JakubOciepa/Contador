using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;

namespace Contador.DAL.MySQL.Repositories
{
	public class IssueRepository : IIssueRepository
	{
		/// <summary>
		/// Add the Contador issue.
		/// </summary>
		/// <param name="issue">Information about issue to add.</param>
		/// <returns>Added issue.</returns>
		public async Task<Result<Issue>> AddIssueAsync(Issue issue)
		{
			return await Task.FromResult(new Result<Issue>(ResponseCode.Ok, issue));
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
