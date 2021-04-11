using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Contador.Web.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : Controller
	{
		private readonly SignInManager<IdentityUser> _signInManager;

		public LoginController(SignInManager<IdentityUser> signInManager)
		{
			_signInManager = signInManager;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginModel login)
		{
			var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

			if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "User name or password is invalid..." });

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, login.Email),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TempConfig.TokenKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiry = DateTime.Now.AddDays(TempConfig.JwtExpiryInDays);

			var token = new JwtSecurityToken(TempConfig.JwtIssuer, TempConfig.JwtAudience, claims, expires: expiry, signingCredentials: creds);

			return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
		}
	}
	public class LoginModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
	
	public class LoginResult
	{
		public bool Successful { get; set; }
		public string Error { get; set; }
		public string Token { get; set; }
	}
}


