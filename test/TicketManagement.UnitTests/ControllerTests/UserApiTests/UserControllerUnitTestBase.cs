using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using TicketManagement.UserAPI.Controllers;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Services.Interfaces;
using TicketManagement.UserAPI.Settings;

namespace TicketManagement.UnitTests.ControllerTests.UserApiTests
{
	/// <summary>
	/// Basic setup for test controller classes UserAPI.
	/// </summary>
	public class UserControllerUnitTestBase
	{
		private protected Mock<IUserService> _mockUserService;
		private protected AdminController _adminController;
		private protected AccountController _accountController;

		private protected List<User> Users { get; } = new List<User>
		{
			new User
			{
				Id = 2, Age = 12, Email = "b@b.ru", Name = "B", Surname = "B",
				Language = "En", Login = "B", Password = "B", Score = 100, UTC = DateTime.UtcNow,
			},
			new User
			{
				Id = 3, Age = 42, Email = "c@c.ru", Name = "C", Surname = "C",
				Language = "En", Login = "C", Password = "C", Score = 100, UTC = DateTime.UtcNow,
			},
			new User
			{
				Id = 1, Age = 12, Email = "a@a.ru", Name = "A", Surname = "A",
				Language = "En", Login = "A", Password = "A", Score = 100, UTC = DateTime.UtcNow,
			},
			new User
			{
				Id = 4, Age = 22, Email = "d@d.ru", Name = "D", Surname = "D",
				Language = "En", Login = "D", Password = "D", Score = 100, UTC = DateTime.UtcNow,
			},
			new User
			{
				Id = 5, Age = 12, Email = "t@t.ru", Name = "T", Surname = "T",
				Language = "En", Login = "T", Password = "T", Score = 100, UTC = DateTime.UtcNow,
			},
		};

		private protected List<User> RegisteredUsers { get; } = new List<User>
		{
			new User
			{
				Id = 1, Age = 42, Email = "c@c.ru", Name = "User", Surname = "User",
				Language = "En", Login = "User", Password = "User", Score = 100, UTC = DateTime.UtcNow,
			},
			new User
			{
				Id = 2, Age = 12, Email = "a@a.ru", Name = "Admin", Surname = "Admin",
				Language = "En", Login = "Admin", Password = "Admin", Score = 100, UTC = DateTime.UtcNow,
			},
		};

		[SetUp]
		public void Setup()
		{
			var jwtHandler = new JwtSecurityTokenHandler();
			_mockUserService = new Mock<IUserService>();
			
			_accountController = new AccountController(
				_mockUserService.Object, new TestTokenSettings(), jwtHandler);
			_adminController = new AdminController(_mockUserService.Object);
		}
	}

	public class TestTokenSettings : IOptions<JwtTokenSettings>
	{
		public JwtTokenSettings Value => new JwtTokenSettings
		{
			SecretKey = "mytestsepersecretkey",
			Audience = "TestAddience",
			Issuer = "IUnique",
			Lifetime = 12,
		};
	}
}
