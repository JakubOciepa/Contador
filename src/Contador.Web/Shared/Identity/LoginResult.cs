namespace Contador.Web.Identity
{
	/// <summary>
	/// The response of the login.
	/// </summary>
	public class LoginResult
	{
		/// <summary>
		/// Defines if the login is succeeded. 
		/// </summary>
		public bool Successful { get; set; }

		/// <summary>
		/// Contains error message of the login.
		/// </summary>
		public string Error { get; set; }

		/// <summary>
		/// Contains token created after correct login.
		/// </summary>
		public string Token { get; set; }
	}
}
