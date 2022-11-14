using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using TicketManagement.ClientModels.Accounts;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Settings;

namespace TicketManagement.UnitTests.ControllerTests.UserApiTests
{
	/// <summary>
	/// Locates unit tests for the UserApi AccountController.
	/// </summary>
	public class AccountControllerTest : UserControllerUnitTestBase
	{
		[Test]
		public async Task Login_NotValidLoginModel_ReturnForbid()
		{
			// Arrange
			string login = "User";
			_mockUserService.Setup(method => method.GetAsync(login))
				.ReturnsAsync(RegisteredUsers.FirstOrDefault(user => user.Login.Equals(login)));
			var loginModel = new LoginModel
			{
				Login = "User",
				Password = "NotValidPassword",
			};

			// Act
			var result = await _accountController.Login(loginModel);

			// Assert
			result.Should().BeOfType<ForbidResult>();
		}

		[Test]
		public async Task Register_ValidRegisterModel_ReturnJsonResult()
		{
			// Arrange
			var registerModel = new RegisterModel
			{
				Age = 20, 
				Email = "test@mail.com",
				Login = "sansay_sa",
				Password = "1234",
				Name = "Alex",
				Surname = "Korneev",
			};
			var user = new User
			{
				Login = registerModel.Login,
				Password = registerModel.Password,
				Name = registerModel.Name,
				Surname = registerModel.Surname,
				Age = registerModel.Age,
				Email = registerModel.Email,
			};
			_mockUserService.Setup(method => method.GetAsync(registerModel.Login))
				.ReturnsAsync(RegisteredUsers.FirstOrDefault(user => user.Login.Equals(registerModel.Login)));
			_mockUserService.Setup(method => method.AddAsync(user));

			// Act
			var result = await _accountController.Register(registerModel);

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

	}
}
