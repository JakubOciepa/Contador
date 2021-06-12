using System;
using System.Net.Http;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using Contador.Services.Interfaces;
using Contador.Web.Client.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace Contador.Web.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			Serilog.Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.BrowserHttp()
				.WriteTo.BrowserConsole()
				.CreateLogger();

			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.Services.AddBlazoredLocalStorage();
			builder.Services.AddAuthorizationCore();
			builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<FilesManager>();
			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddSingleton<ILog>(log => new Services.Log());

			Serilog.Log.Logger.Information("Warming up!");

			await builder.Build().RunAsync();
		}
	}
}
