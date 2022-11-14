using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TicketManagement.UserAPI.DataContext;
using TicketManagement.UserAPI.MiddlewareExtension;
using TicketManagement.UserAPI.Repositories;
using TicketManagement.UserAPI.Repositories.Interfaces;
using TicketManagement.UserAPI.Services;
using TicketManagement.UserAPI.Services.Interfaces;
using TicketManagement.UserAPI.Settings;

namespace TicketManagement.UserAPI
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
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			var tokenSettings = Configuration.GetSection(nameof(JwtTokenSettings));

			services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ITicketRepository, TicketRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IUserService, UserService>();

			services.Configure<JwtTokenSettings>(tokenSettings);
			services.AddScoped(options => new JwtSecurityTokenHandler());
			
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
					options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
				});

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

			services.AddControllers();
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "UserAPI Swagger docs",
					Version = "v1",
				});
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					Description = "Jwt Token is required to access the endpoints",
					In = ParameterLocation.Header,
					Name = "JWT Authentication",
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme,
					},
				};

				options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{ jwtSecurityScheme, Array.Empty<string>() },
				});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseTokenTransferring();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseLogRequest();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
