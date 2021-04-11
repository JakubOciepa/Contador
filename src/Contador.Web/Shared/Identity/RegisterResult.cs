using System.Collections.Generic;

namespace Contador.Web.Identity
{
	public class RegisterResult
	{
		public bool Successful { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
