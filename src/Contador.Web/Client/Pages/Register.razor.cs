using System.Collections.Generic;
using System.Threading.Tasks;

using Contador.Web.Client.Services;
using Contador.Web.Identity;

using Microsoft.AspNetCore.Components;

namespace Contador.Web.Client.Pages
{
	public partial class Register
	{
		[Inject] private IAuthService _authService { get; set; }
		[Inject] private NavigationManager _navigationManager { get; set; }

		private RegisterModel RegisterModel = new RegisterModel();
		private bool ShowErrors;
		private IEnumerable<string> Errors;

		private async Task HandleRegistration()
		{
			ShowErrors = false;

			var result = await _authService.Register(RegisterModel);

			if (result.Successful)
			{
				_navigationManager.NavigateTo("/login");
			}
			else
			{
				Errors = result.Errors;
				ShowErrors = true;
			}
		}

	}
}
