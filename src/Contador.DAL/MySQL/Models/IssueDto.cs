
using Contador.Core.Models;

namespace Contador.DAL.MySQL.Models
{
	public class IssueDto : Issue
	{
		public static class ParameterName
		{
			public const string Name = "name_p";
		}

		public static class ProcedureName
		{
			public const string GetAll = "issue_getAll";

			public const string Add = "issue_Add";
		}
	}
}
