using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using Newtonsoft.Json;
using TicketManagement.WebApplication.Clients;
using TicketManagement.ClientModels.Accounts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides login and legalization operations.
	/// </summary>
	public class AccountController : Controller
	{
		private readonly IUserRestClient _userClient;

		public AccountController(IUserRestClient userClient)
		{
			_userClient = userClient;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				string token;
				try
				{
					token = await _userClient.Register(model);
				}
				catch
				{
					ModelState.AddModelError("", $"Login '{model.Login}' is bussy");
					return View(model);
				}

				HttpContext.Response.Cookies.Append("Token", JsonConvert.DeserializeObject<string>(token));
				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Login()
		{
			var role = ((ClaimsIdentity)User.Identity).Claims
				.FirstOrDefault(x => x.Type == ClaimTypes.Role);

			if (role != null && role.Value.Equals(RoleState.Admin.ToString()))
			{
				return RedirectToAction("Index", "Admin");
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				string token;
				try
				{
					token = await _userClient.Login(model);
				}
				catch
				{
					ModelState.AddModelError("", "Incorrect login or password");
					return View(model);
				}

				HttpContext.Response.Cookies.Append("Token", JsonConvert.DeserializeObject<string>(token));
				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Exit()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Response.Cookies.Delete("Token");
			return RedirectToAction("Login", "Account");
		}
	}
}
