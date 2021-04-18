using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Contador.Web.Identity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Contador.Web.Server.Controllers
{
	/// <summary>
	/// Provides the login mechanism.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : Controller
	{
		private readonly SignInManager<IdentityUser> _signInManager;

		/// <summary>
		/// Creates an instance of the <see cref="LoginController"/> class.
		/// </summary>
		/// <param name="signInManager">Sign in manager.</param>
		public LoginController(SignInManager<IdentityUser> signInManager)
		{
			_signInManager = signInManager;
		}

		/// <summary>
		/// Log in the provided user. 
		/// </summary>
		/// <param name="login">User credentials to log in.</param>
		/// <returns>
		/// <para><see cref="StatusCodes.Status200OK"/> if the log in succeeded.</para>
		/// <para><see cref="StatusCodes.Status400BadRequest"/> if credentials are not correct.</para>
		/// </returns> 
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Login([FromBody] LoginModel login)
		{
			var result = await _signInManager.PasswordSignInAsync(login.Name, login.Password, false, false);

			if (result.Succeeded is false)
			{
				return BadRequest(new LoginResult
				{
					Successful = false,
					Error = "User name or password is invalid..."
				});
			}

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, login.Name),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TempConfig.TokenKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiry = DateTime.Now.AddDays(TempConfig.JwtExpiryInDays);

			var token = new JwtSecurityToken(TempConfig.JwtIssuer, TempConfig.JwtAudience, claims, expires: expiry, signingCredentials: creds);

			return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
		}
	}
}


