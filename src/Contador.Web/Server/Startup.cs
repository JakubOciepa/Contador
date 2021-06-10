using System;
using System.Data;
using System.IO;
using System.Text;

using Contador.DAL;
using Contador.DAL.Abstractions;
using Contador.DAL.MySql.Repositories;
using Contador.DAL.MySQL.Repositories;
using Contador.Services;
using Contador.Services.Interfaces;
using Contador.Web.Server.Identity;
using Contador.Web.Server.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using MySql.Data.MySqlClient;

namespace Contador.Web.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contador.Api", Version = "v1" }));
			services.AddTransient<IDbConnection>(db =>
				new MySqlConnection($"server=localhost;{Configuration["DbContent"]}"));

			services.AddDbContext<IdentityDatabaseContext>(options
				=> options.UseMySql($"server=localhost;{Configuration["DbUsers"]}",
				new MySqlServerVersion(new Version(10, 3, 25)), mySqlOptions
					=> mySqlOptions.CharSetBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.CharSetBehavior.NeverAppend))
					.EnableSensitiveDataLogging().EnableDetailedErrors());

			services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<IdentityDatabaseContext>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = TempConfig.JwtIssuer,
					ValidAudience = TempConfig.JwtAudience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TempConfig.TokenKey))
				};
			});


			services.AddSingleton<ILog, Log>();
			services.AddScoped<IExpenseRepository, ExpenseRepository>();
			services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IExpenseCategoryService, ExpenseCategoryManager>();
			services.AddScoped<IExpenseService, ExpenseManager>();
			services.AddScoped<IExpenseManager, ExpenseManager>();
			services.AddScoped<IExpenseCategoryManager, ExpenseCategoryManager>();
			services.AddScoped<IReportService, ReportService>();
			services.AddScoped<IIssueRepository, IssueRepository>();
			services.AddScoped<IIssueService, IssueManager>();
			services.AddControllers();
			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contador.Api v1"));
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			//app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(Directory.GetCurrentDirectory(), "Files")),
				RequestPath = "/Files"
			});

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}

	public static class TempConfig
	{
		public static readonly string TokenKey = @"um+:$^ie4z%,`,d'i]sr4$/,3e]qu!>7q*h=n!/<n=b*eg{ogf}a~f7<j<moz7~z";
		public static readonly string JwtIssuer = "https://localhost";
		public static readonly string JwtAudience = "https://localhost";
		public static readonly int JwtExpiryInDays = 1;
	}
}
