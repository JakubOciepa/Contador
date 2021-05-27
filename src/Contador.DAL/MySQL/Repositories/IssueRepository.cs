using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Contador.Core.Models;
using Contador.Core.Utils.Extensions;
using Contador.DAL.MySQL.Models;

using Dapper;

namespace Contador.DAL.MySQL.Repositories
{
	public class IssueRepository : IIssueRepository
	{
		private readonly IDbConnection _dbConnection;

		public IssueRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		/// <summary>
		/// Add the Contador issue.
		/// </summary>
		/// <param name="issue">Information about issue to add.</param>
		/// <returns>Added issue.</returns>
		public async Task<Issue> AddAsync(Issue issue)
		{
			var parameter = new DynamicParameters();
			parameter.Add(IssueDto.ParameterName.Name, issue.Name);

			return (await _dbConnection.QuerySingleAsync<IssueDto>(IssueDto.ProcedureName.Add,
				parameter, commandType: CommandType.StoredProcedure)
				.CAF());
		}

		/// <summary>
		/// Gets all created issues.
		/// </summary>
		/// <returns>List of all available issues.</returns>
		public async Task<IEnumerable<Issue>> GetAllAsync()
		{
			return (await _dbConnection.QueryAsync<IssueDto>(IssueDto.ProcedureName.GetAll,
				commandType: CommandType.StoredProcedure).CAF()).Cast<Issue>().ToList();
		}
	}
}
