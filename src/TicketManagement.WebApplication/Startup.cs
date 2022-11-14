using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using RestEase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicketManagement.WebApplication.Settings;
using TicketManagement.WebApplication.Clients;
using TicketManagement.WebApplication.MiddlewareExtension;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.FeatureManagement;
using TicketManagement.WebApplication.FeatureHandlers;

namespace TicketManagement.WebApplication
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
			services.AddScoped(scope =>
			{
				var baseUrl = Configuration["PurchaseApiAddress"];
				return RestClient.For<IPurchaseRestClient>(baseUrl);
			});

			services.AddFeatureManagement()
				.UseDisabledFeaturesHandler(new RedirectDisabledFeatureHandler(Configuration["ReactAppAddress"]));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
					options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
				});

			var tokenSettings = Configuration.GetSection(nameof(JwtTokenSettings));
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization();
			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
				{
					new CultureInfo("be-BY"),
					new CultureInfo("en-US"),
					new CultureInfo("ru-RU"),
				};
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
				options.DefaultRequestCulture = new RequestCulture("en-US");
			});

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
			IOptions<RequestLocalizationOptions> localisationOptions)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");			
				app.UseHsts();
			}

			app.UseLanguageTransfer();

			app.UseRequestLocalization(localisationOptions.Value);

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseTokenTransferring();

			app.UseStatusCodePages(async context => 
			{
				var response = context.HttpContext.Response;
				var request = context.HttpContext.Request;

				if (response.StatusCode == 403 || response.StatusCode == StatusCodes.Status401Unauthorized)
				{
					await Task.Run(() => response.Redirect("/account/login"));
				}
			});

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseLogRequest();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
