using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.ClientModels.Accounts;
using TicketManagement.ClientModels.Tickets;
using TicketManagement.ClientModels.Users;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Services.Interfaces;

namespace TicketManagement.UserAPI.Controllers
{
	/// <summary>
	/// Provides methods for managing user account.
	/// Available to users with roles User, ModeratorEvent, ModeratorVenue.
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	[Authorize(Roles = "User, ModeratorEvent, ModeratorVenue")]
	public class UserController : Controller
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("User")]
		public async Task<IActionResult> GetUser()
		{
			var user = await _userService.GetAsync(User.Identity.Name);
			var roles = await _userService.GetUserRoles(user.Id);
			var tickets = await _userService.GetUserTicketsAsync(user.Id);

			var userViewModel = new UserTicketsModel
			{
				User = new UserModel
				{
					Id = user.Id,
					Name = user.Name,
					Surname = user.Surname,
					Email = user.Email,
					Age = user.Age,
					Login = user.Login,
					Password = user.Password,
					Score = user.Score.ToString(),
					State = (RoleState)roles.First().Id,
					RoleName = roles.First().Name,
				},

				Tickets = tickets.Select(x => new TicketViewModel
				{
					Price = x.Price,
					EventName = x.EventName,
					DateTimePurchase = x.DateTimePurchase,
				}).ToList(),
			};

			return Json(userViewModel);
		}

		[HttpGet("UserBalance")]
		public async Task<IActionResult> GetUserBalance()
		{
			var user = await _userService.GetAsync(User.Identity.Name);
			if (user == null)
			{
				return NotFound();
			}

			var userModel = new UserModel
			{
				Id = user.Id,
				Score = user.Score.ToString(),
			};

			return Json(userModel);
		}

		[HttpPut("UserBalance")]
		public async Task<IActionResult> TopUpUserBalance([FromBody] UserModel model)
		{
			await _userService.AddScore(model.Id, Convert.ToDouble(model.Score));
			return Ok();
		}

		[HttpGet("UserSettings")]
		public async Task<IActionResult> GetUserSettings()
		{
			var user = await _userService.GetAsync(User.Identity.Name);
			if (user == null)
			{
				return NotFound();
			}

			var userModel = new UserModel
			{
				Id = user.Id,
				Login = user.Login,
				Age = user.Age,
				Email = user.Email,
				Name = user.Name,
				Surname = user.Surname,
				Password = user.Password,
			};

			return Json(userModel);
		}

		[HttpPut("UserSettings")]
		public async Task<IActionResult> UpdateUserSettings([FromBody] UserModel model)
		{
			var selectedUser = await _userService.GetAsync(User.Identity.Name);

			var user = new User
			{
				Id = selectedUser.Id,
				Login = model.Login,
				Score = selectedUser.Score,
				Age = model.Age,
				Email = model.Email,
				Password = model.Password,
				Name = model.Name,
				Surname = model.Surname,
			};

			await _userService.Update(user);
			return Ok();
		}
	}
}
