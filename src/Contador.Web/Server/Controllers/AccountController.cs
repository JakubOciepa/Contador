using System.Linq;
using System.Threading.Tasks;

using Contador.Web.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;

		public AccountController(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] RegisterModel model)
		{
			var newUser = new IdentityUser { UserName = model.Name, Email = model.Email };

			var result = await _userManager.CreateAsync(newUser, model.Password);

			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(x => x.Description);

				return Ok(new RegisterResult { Successful = false, Errors = errors });
			}

			return Ok(new RegisterResult { Successful = true });
		}
	}
}
