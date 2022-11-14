using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Users;
using TicketManagement.WebApplication.Clients;
using TicketManagement.WebApplication.Settings;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides operations for managing a user account.
	/// Not available for Admin role.
	/// </summary>
	[FeatureGate(FeatureFlags.UseOnlyMvc)]
	[Authorize(Roles = "User, ModeratorEvent, ModeratorVenue")]
	public class UserController : Controller
	{
		private readonly IUserRestClient _userClient;

		public UserController(IUserRestClient userClient)
		{
			_userClient = userClient;
		}

		public async Task<IActionResult> Index()
		{
			var model = await _userClient.GetUser(HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> UpdateSettings()
		{
			var model = await _userClient.GetUserSettings(HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateSettings(UserModel model)
		{
			await _userClient.UpdateUserSettings(model, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Exit", "Account");
		}

		[HttpGet]
		public async Task<IActionResult> UpBalance()
		{
			var viewModel = await _userClient.GetUserBalance(HttpContext.Request.Cookies["Token"]);
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> UpBalance(UserModel model)
		{
			await _userClient.TopUpUserBalance(model, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Index", "Home");
		}
	}
}
