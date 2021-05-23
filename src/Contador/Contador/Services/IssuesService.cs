using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Core.Common;
using Contador.Core.Models;
using Contador.Services.Interfaces;

namespace Contador.Services
{
	public class IssuesService : IIssuesService
	{
		public Task<Result<Issue>> AddIssueAsync(Issue issue)
		{
			throw new System.NotImplementedException();
		}

		public Task<Result<IEnumerable<Issue>>> GetAllIssuesAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
