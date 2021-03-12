using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Contador.Abstractions;
using Contador.Web.Client.Services;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddSingleton<ILog>(log => new Services.Log());

			Serilog.Log.Logger.Information("Warming up!");

            await builder.Build().RunAsync();
        }
    }
}
