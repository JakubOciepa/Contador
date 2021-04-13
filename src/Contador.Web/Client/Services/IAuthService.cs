using System.Threading.Tasks;

using Contador.Web.Identity;

namespace Contador.Web.Client.Services
{
	/// <summary>
	/// Provides methods to auth the user.
	/// </summary>
	public interface IAuthService
	{
		/// <summary>
		/// Registers the user by the provided model.
		/// </summary>
		/// <param name="registerModel">Data of the user to register.</param>
		/// <returns>Result of the user registration.</returns>
		Task<RegisterResult> Register(RegisterModel registerModel);

		/// <summary>
		/// Log in the user by provided model.
		/// </summary>
		/// <param name="loginModel">Data of the user to log in.</param>
		/// <returns>Result of the log in.</returns>
		Task<LoginResult> Login(LoginModel loginModel);

		/// <summary>
		/// Logout the logged user.
		/// </summary>
		Task Logout();
	}
}
