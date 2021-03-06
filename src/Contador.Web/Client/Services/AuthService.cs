﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using Contador.Web.Identity;

using Microsoft.AspNetCore.Components.Authorization;

namespace Contador.Web.Client.Services
{
	/// <summary>
	/// Provides methods to auth the user.
	/// </summary>
	public class AuthService : IAuthService
	{

		private readonly HttpClient _httpClient;
		private readonly AuthenticationStateProvider _authenticationStateProvider;
		private readonly ILocalStorageService _localStorage;

		public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider,
			ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_authenticationStateProvider = authenticationStateProvider;
			_localStorage = localStorage;
		}

		/// <summary>
		/// Registers the user by the provided model.
		/// </summary>
		/// <param name="registerModel">Data of the user to register.</param>
		/// <returns>Result of the user registration.</returns>
		public async Task<RegisterResult> Register(RegisterModel registerModel)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "api/account");

			var body = new
			{
				name = registerModel.Name,
				email = registerModel.Email,
				password = registerModel.Password,
				confirmPassword = registerModel.ConfirmPassword
			};

			var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
			request.Content = content;
			var result = await _httpClient.SendAsync(request);

			return await result.Content.ReadFromJsonAsync<RegisterResult>();
		}

		/// <summary>
		/// Log in the user by provided model.
		/// </summary>
		/// <param name="loginModel">Data of the user to log in.</param>
		/// <returns>Result of the log in.</returns>
		public async Task<LoginResult> Login(LoginModel loginModel)
		{
			var loginAsJson = JsonSerializer.Serialize(loginModel);

			var response = await _httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

			var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			if (response.IsSuccessStatusCode is false)
			{
				return loginResult;
			}

			await _localStorage.SetItemAsync("authToken", loginResult.Token);
			((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Name);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

			return loginResult;
		}

		/// <summary>
		/// Logout the logged user.
		/// </summary>
		public async Task Logout()
		{
			await _localStorage.RemoveItemAsync("authToken");
			((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
			_httpClient.DefaultRequestHeaders.Authorization = null;
		}
	}
}
