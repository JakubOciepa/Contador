using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
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

	public class RegisterModel
	{
		[Required]
		[Display(Name="Name")]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}


	public class RegisterResult
	{
		public bool Successful { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
