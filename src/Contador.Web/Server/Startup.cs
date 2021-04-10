using System.Data;

using Contador.Abstractions;
using Contador.DAL.Abstractions;
using Contador.DAL.MySql.Repositories;
using Contador.Services;
using Contador.Web.Server.Identity;
using Contador.Web.Server.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
				new MySqlConnection($"server=localhost;{Configuration["DbCredentials"]}"));

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
				};
			});


			services.AddSingleton<ILog, Log>();
			services.AddScoped<IExpenseRepository, ExpenseRepository>();
			services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IExpenseCategoryService, ExpenseCategoryService>();
			services.AddScoped<IExpenseService, ExpenseService>();
			services.AddScoped<IExpenseManager, ExpenseService>();
			services.AddScoped<IExpenseCategoryManager, ExpenseCategoryService>();
			services.AddScoped<IReportService, ReportService>();
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

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
