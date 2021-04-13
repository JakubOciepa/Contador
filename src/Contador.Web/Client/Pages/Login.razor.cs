using System.Threading.Tasks;

using Contador.Web.Client.Services;
using Contador.Web.Identity;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Pages
{
	public partial class Login
	{
		[Inject] private IAuthService _authService { get; set; }
		[Inject] private NavigationManager _navigationManager { get; set; }

		private LoginModel LoginModel = new();
		private bool ShowErrors;
		private string Error = "";

		private async Task HandleLogin()
		{
			ShowErrors = false;

			var result = await _authService.Login(LoginModel);

			if (result.Successful)
			{
				_navigationManager.NavigateTo("/");
			}
			else
			{
				Error = result.Error;
				ShowErrors = true;
			}
		}
	}
}
