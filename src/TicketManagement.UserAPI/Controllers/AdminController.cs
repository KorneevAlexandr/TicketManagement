using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.ClientModels.Accounts;
using TicketManagement.ClientModels.Users;
using TicketManagement.UserAPI.Services.Interfaces;

namespace TicketManagement.UserAPI.Controllers
{
	/// <summary>
	/// Provides methods for viewing user credentials and managing their roles.
	/// Available to a user with a role Admin.
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private const int USERS_ON_PAGE = 3;
		private readonly IUserService _userService;

		public AdminController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("Users")]
		public async Task<IActionResult> GetAllUsers(int roleId)
		{
			roleId = roleId == 0 ? (int)RoleState.User : roleId;
			int page = HttpContext != null ? Convert.ToInt32(HttpContext.Request.Headers["Page"]) : 1;
			var usersCount = await _userService.CountAsync(roleId);
			var users = await _userService.GetAllAsync(roleId, (page - 1) * USERS_ON_PAGE, USERS_ON_PAGE);
			var roleName = await _userService.GetRole(roleId);

			if (!users.Any())
			{
				return Json(new UsersForAdminModel { Users = new List<UserModel>(), RoleId = new int[] { roleId } });
			}

			var model = new UsersForAdminModel
			{
				RoleId = new int[] { roleId },
				NumberActivityPage = page,
				NumberPages = usersCount % USERS_ON_PAGE == 0 ? usersCount / USERS_ON_PAGE
					: ((usersCount / USERS_ON_PAGE) + 1),
				Users = users.Select(user => new UserModel
				{
					Id = user.Id,
					FullName = $"{user.Surname} {user.Name}",
					Login = user.Login,
					Password = user.Password,
					RoleName = roleName,
				}),
			};

			return Json(model);
		}

		[HttpGet("User")]
		public async Task<IActionResult> GetUserDetails(int userId)
		{
			var user = await _userService.GetAsync(userId);
			if (user != null)
			{
				var userRoles = await _userService.GetUserRoles(user.Id);
				var userModel = new UserModel
				{
					Id = user.Id,
					FullName = $"{user.Surname} {user.Name}",
					Login = user.Login,
					Password = user.Password,
					RoleName = userRoles.First().Name,
				};
				return Json(userModel);
			}

			return Forbid();
		}

		[HttpPut("UserRole")]
		public async Task<IActionResult> UpdateUserRole(int userId)
		{
			var user = await _userService.GetAsync(userId);
			if (user != null)
			{
				var roleId = Convert.ToInt32(HttpContext.Request.Headers["RoleId"]);
				await _userService.UpdateRole(userId, roleId);

				return Ok();
			}

			return Forbid();
		}
	}
}
