using System.Threading.Tasks;

using Contador.Web.Identity;

namespace Contador.Web.Client.Services
{
	public interface IAuthService
	{
		Task<RegisterResult> Register(RegisterModel registerModel);
		Task<LoginResult> Login(LoginModel loginModel);
		Task Logout();
	}
}
