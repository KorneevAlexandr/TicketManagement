using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Users;
using TicketManagement.ReactUI.ApiClient;

namespace TicketManagement.ReactUI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private const string LanguageCookieKey = "language";
		private readonly IUserRestClient _userClient;

		public UserController(IUserRestClient userClient)
		{
			_userClient = userClient;
		}

		[HttpGet("username")]
		public async Task<IActionResult> GetUsername()
		{
			return await Task.Run(() => new JsonResult(User.Identity.Name));
		}

		[HttpGet("user")]
		public async Task<IActionResult> GetUser()
		{
			var user = await _userClient.GetUser(HttpContext.Request.Cookies["Token"]);
			return new JsonResult(user.User);
		}

		[HttpGet("userTickets")]
		public async Task<IActionResult> GetUserTickets()
		{
			var user = await _userClient.GetUser(HttpContext.Request.Cookies["Token"]);
			return new JsonResult(user.Tickets);
		}

		[HttpGet("userSettings")]
		public async Task<IActionResult> GetUserSettings()
		{
			var userSettings = await _userClient.GetUserSettings(HttpContext.Request.Cookies["Token"]);
			return new JsonResult(userSettings);
		}

		[HttpPut("userBalance")]
		public async Task<IActionResult> UpdateUserBalance(UserModel model)
		{
			await _userClient.TopUpUserBalance(model, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(model);
		}

		[HttpPut("userSettings")]
		public async Task<IActionResult> UpdateUserSettings(UserModel model)
		{
			await _userClient.UpdateUserSettings(model, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(model);
		}

		[HttpPost("exit")]
		public async Task<IActionResult> Exit()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Response.Cookies.Delete("Token");
			return new OkResult();
		}
	}
}
