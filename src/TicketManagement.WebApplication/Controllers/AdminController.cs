using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Accounts;
using TicketManagement.WebApplication.Clients;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides operations for viewing information about users and changing their roles.
	/// Only available to users of the Admin role.
	/// </summary>
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly IUserRestClient _userClient;

		public AdminController(IUserRestClient userClient)
		{
			_userClient = userClient;
		}

		public IActionResult Index()
		{
			return Redirect($"~/Admin/AllUsers?roleId={(int)RoleState.User}&page=1");
		}

		[HttpPost]
		public IActionResult AllUsers()
		{
			return Redirect($"~/Admin/AllUsers?roleId={(int)RoleState.User}&page=1");
		}

		[HttpGet]
		public async Task<IActionResult> AllUsers(int roleId, int? page)
		{
			page ??= 1;
			var responseModel = await _userClient.GetAllUsers(roleId, page.Value, HttpContext.Request.Cookies["Token"]);

			return View(responseModel);
		}

		[HttpPost]
		public IActionResult AllVenueModerators()
		{
			return Redirect($"~/Admin/AllUsers?roleId={(int)RoleState.ModeratorVenue}&page=1");
		}

		[HttpPost]
		public IActionResult AllEventModerators()
		{
			return Redirect($"~/Admin/AllUsers?roleId={(int)RoleState.ModeratorEvent}&page=1");
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(int? id)
		{
			return await Update(id, (int)RoleState.User);
		}

		[HttpPost]
		public async Task<IActionResult> CreateModeratorVenue(int? id)
		{
			return await Update(id, (int)RoleState.ModeratorVenue);
		}

		[HttpPost]
		public async Task<IActionResult> CreateModeratorEvent(int? id)
		{
			return await Update(id, (int)RoleState.ModeratorEvent);
		}

		private async Task<IActionResult> Update(int? id, int roleId)
		{
			if (id == null)
			{
				return NotFound();
			}

			await _userClient.UpdateUserRole(id.Value, roleId, HttpContext.Request.Cookies["Token"]);
			return Redirect($"~/Admin/AllUsers?roleId={roleId}&page=1");
		}

		[HttpGet]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var user = await _userClient.GetUserDetails(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(user);
		}
	}
}
