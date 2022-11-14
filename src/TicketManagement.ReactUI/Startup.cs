using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RestEase;
using System.Text;
using TicketManagement.ReactUI.ApiClient;
using TicketManagement.ReactUI.Controllers;
using TicketManagement.ReactUI.MiddlewareExtension;
using TicketManagement.ReactUI.Settings;

namespace TicketManagement.ReactUI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped(scope =>
			{
				var baseUrl = Configuration["UserApiAddress"];
				return RestClient.For<IUserRestClient>(baseUrl);
			});
			services.AddScoped(scope =>
			{
				var baseUrl = Configuration["EventApiAddress"];
				return RestClient.For<IEventRestClient>(baseUrl);
			});
			services.AddScoped(scope =>
			{
				var baseUrl = Configuration["VenueApiAddress"];
				return RestClient.For<IVenueRestClient>(baseUrl);
			});

			var tokenSettings = Configuration.GetSection(nameof(JwtTokenSettings));
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddCookie()
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = tokenSettings[nameof(JwtTokenSettings.Issuer)],
						ValidateAudience = true,
						ValidAudience = tokenSettings[nameof(JwtTokenSettings.Audience)],
						ValidateLifetime = true,

						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings[nameof(JwtTokenSettings.SecretKey)])),
						ValidateIssuerSigningKey = true,
					};
				});

			services.AddControllersWithViews();

			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/build";
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSpaStaticFiles();

			app.UseRouting();

			app.UseTokenTransferring();		
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseLogRequest();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseReactDevelopmentServer(npmScript: "start");
				}
			});
		}
	}
}
