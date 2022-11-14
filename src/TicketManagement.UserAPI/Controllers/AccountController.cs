using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TicketManagement.ClientModels.Accounts;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Services.Interfaces;
using TicketManagement.UserAPI.Settings;

namespace TicketManagement.UserAPI.Controllers
{
	/// <summary>
	/// Provides methods for authorizing and registering users.
	/// Available for unauthorized users.
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class AccountController : Controller
	{
		private readonly IUserService _userService;
		private readonly IOptions<JwtTokenSettings> _tokenSettings;
		private readonly JwtSecurityTokenHandler _jwtHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountController"/> class.
		/// </summary>
		/// <param name="userService">User service.</param>
		/// <param name="tokenSettings">Token settings.</param>
		/// <param name="jwtHandler">Jwt handler.</param>
		public AccountController(IUserService userService, IOptions<JwtTokenSettings> tokenSettings,
			JwtSecurityTokenHandler jwtHandler)
		{
			_userService = userService;
			_tokenSettings = tokenSettings;
			_jwtHandler = jwtHandler;
		}

		/// <summary>
		/// Method for user login in the system.
		/// </summary>
		/// <param name="model">Login model.</param>
		/// <returns>If success - Jwt-token in a json-format, else - Forbid.</returns>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var user = await _userService.GetAsync(model.Login);

			if (user == null || !user.Password.Equals(model.Password))
			{
				return Forbid();
			}

			var roles = await _userService.GetUserRoles(user.Id);
			var token = GenerateToken(model.Login, roles.First().Name);

			return Json(token);
		}

		/// <summary>
		/// Method for user register in the system.
		/// </summary>
		/// <param name="model">Login model.</param>
		/// <returns>If success - Jwt-token in a json-format, else - Forbid.</returns>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			var user = await _userService.GetAsync(model.Login);
			if (user == null)
			{
				user = new User
				{
					Login = model.Login,
					Password = model.Password,
					Name = model.Name,
					Surname = model.Surname,
					Age = model.Age,
					Email = model.Email,
				};

				await _userService.AddAsync(user);
			}
			else
			{
				return Forbid();
			}

			var token = GenerateToken(model.Login, RoleState.User.ToString());

			return Json(token);
		}

		/// <summary>
		/// Generate jwt token for concreate user, used jwt-authentication.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <param name="role">User role.</param>
		/// <returns>Jwt token.</returns>
		private string GenerateToken(string login, string role)
		{
			var userIdentity = GetIdentity(login, role);

			var now = DateTime.UtcNow;
			// создаем JWT-токен
			var jwt = new JwtSecurityToken(
					issuer: _tokenSettings.Value.Issuer,
					audience: _tokenSettings.Value.Audience,
					notBefore: now,
					claims: userIdentity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(_tokenSettings.Value.Lifetime)),
					signingCredentials: new SigningCredentials(
						new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenSettings.Value.SecretKey)),
						SecurityAlgorithms.HmacSha256));

			var encodedJwt = _jwtHandler.WriteToken(jwt);

			return encodedJwt;
		}

		/// <summary>
		/// Getting claims identity for concreate user-login and user-role.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <param name="role">User role.</param>
		/// <returns>Object ClaimsIdentity.</returns>
		private ClaimsIdentity GetIdentity(string login, string role)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, login),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
			};

			var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);
			return claimsIdentity;
		}

	}
}
