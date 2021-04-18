using System.Collections.Generic;

namespace Contador.Web.Identity
{
	/// <summary>
	/// Contains result data of the account registration.
	/// </summary>
	public class RegisterResult
	{
		/// <summary>
		/// Indicates if the registration succeeded. 
		/// </summary>
		public bool Successful { get; set; }
	
		/// <summary>
		/// Contains errors generated while account registration. 
		/// </summary>
		public IEnumerable<string> Errors { get; set; }
	}
}
