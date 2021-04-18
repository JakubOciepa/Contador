using System.Linq;
using System.Threading.Tasks;

using Contador.Web.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Contador.Web.Server.Controllers
{
	/// <summary>
	/// Provides functions to manage users.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;

		/// <summary>
		/// Creates instance of the <see cref="AccountController"/> class.
		/// </summary>
		/// <param name="userManager">User manager which will handle user creation.</param>
		public AccountController(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		/// <summary>
		/// Creates an account for the user by the provided model.
		/// </summary>
		/// <param name="model">Model by which the account will be created.</param>
		/// <returns>Succeeded = true if account is created or false with errors is there was some.</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Post([FromBody] RegisterModel model)
		{
			var newUser = new IdentityUser { UserName = model.Name, Email = model.Email };

			var result = await _userManager.CreateAsync(newUser, model.Password);

			if (result.Succeeded is false)
			{
				var errors = result.Errors.Select(x => x.Description);

				return Ok(new RegisterResult { Successful = false, Errors = errors });
			}

			return Ok(new RegisterResult { Successful = true });
		}

		[HttpGet("{userName}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IdentityUser>> GetByName([FromRoute]string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);

			return user is IdentityUser ? Ok(user) : BadRequest("User not found.");
		}
	}
}
