using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using Contador.Abstractions;

using Microsoft.AspNetCore.Components.Authorization;

namespace Contador.Web.Client.Services
{
	/// <summary>
	/// Provides methods to authenticate the user.
	/// </summary>
	public class ApiAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		/// <summary>
		/// Creates an instance of the <see cref="ApiAuthenticationStateProvider"/> class.
		/// </summary>
		/// <param name="httpClient">HTTP client.</param>
		/// <param name="localStorage">Local storage to store the token.</param>
		public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		/// <summary>
		/// Gets the state of the authentication of the user, if any.
		/// </summary>
		/// <returns>The <see cref="AuthenticationState"/> of the user, if any.</returns>
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var savedToken = await _localStorage.GetItemAsync<string>("authToken");

			if (string.IsNullOrWhiteSpace(savedToken))
			{
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityToken(savedToken).Claims, "jwt")));
		}

		/// <summary>
		/// Mark the user as authenticated (logged in).
		/// </summary>
		/// <param name="name">The user name to mark as authenticated</param>
		public void MarkUserAsAuthenticated(string name)
		{
			var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, name) }, "apiauth"));
			var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
			NotifyAuthenticationStateChanged(authState);
		}

		/// <summary>
		/// Mark the user as logged out from application.
		/// </summary>
		public void MarkUserAsLoggedOut()
		{
			var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
			var authState = Task.FromResult(new AuthenticationState(anonymousUser));
			NotifyAuthenticationStateChanged(authState);
		}
	}
}
