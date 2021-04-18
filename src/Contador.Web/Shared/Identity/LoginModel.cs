using System.ComponentModel.DataAnnotations;

namespace Contador.Web.Identity
{
	/// <summary>
	/// Defines properties required to correct login.
	/// </summary>
	public class LoginModel
	{
		/// <summary>
		/// The name of the user to login.
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// The password for the account to login.
		/// </summary>
		[Required]
		public string Password { get; set; }
	}
}
