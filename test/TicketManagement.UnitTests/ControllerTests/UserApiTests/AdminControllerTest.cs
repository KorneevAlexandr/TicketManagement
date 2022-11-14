using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TicketManagement.UserAPI.Models;

namespace TicketManagement.UnitTests.ControllerTests.UserApiTests
{
	/// <summary>
	/// Locates unit tests for the UserApi AdminController.
	/// </summary>
	public class AdminControllerTest : UserControllerUnitTestBase
	{
		[Test]
		public async Task GetAllUsers_RoleValid_ReturnJsonResult()
		{
			// Arrange
			int roleId = 1;

			// Act
			var result = await _adminController.GetAllUsers(roleId);

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

		[Test]
		public async Task GetUserDetails_IdNotExist_ReturnForbid()
		{
			// Arrange
			var badId = 0;
			_mockUserService.Setup(method => method.GetAsync(badId))
				.ReturnsAsync(Users.FirstOrDefault(user => user.Id == badId));

			// Act
			var result = await _adminController.GetUserDetails(badId);

			// Assert
			result.Should().BeOfType<ForbidResult>();
		}

		[Test]
		public async Task GetUserDetails_IdExist_ReturnUserId()
		{
			// Arrange
			var userId = 2;
			_mockUserService.Setup(method => method.GetUserRoles(userId))
				.ReturnsAsync(new Role[] { new Role { Id = 2, Name = "User" } });
			_mockUserService.Setup(method => method.GetAsync(userId))
				.ReturnsAsync(Users.First());

			// Act
			var result = await _adminController.GetUserDetails(userId);

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

		[Test]
		public async Task UpdateUserRole_IdNotExist_ReturnForbid()
		{
			// Arrange
			var badId = 0;
			_mockUserService.Setup(method => method.GetAsync(badId))
				.ReturnsAsync(Users.FirstOrDefault(user => user.Id == badId));

			// Act
			var result = await _adminController.UpdateUserRole(badId);

			// Assert
			result.Should().BeOfType<ForbidResult>();
		}
	}
}
