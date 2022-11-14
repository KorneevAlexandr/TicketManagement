using System;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.PurchaseAPI.Controllers;

namespace TicketManagement.UnitTests.ControllerTests.PurchaseApiTests
{
	/// <summary>
	/// Basic setup for test controller classes PurchaseAPI.
	/// </summary>
	public class PurchaseControllerUnitTestBase
	{
		private protected readonly EventDto _testEvent = new EventDto
		{
			Id = 1,
			DateTimeEnd = DateTime.Now,
			DateTimeStart = DateTime.Now,
			Description = "Good",
			Name = "Hey!",
			LayoutId = 1,
			Price = 100,
			State = SeatStateDto.Free,
			URL = "my_path",
			VenueId = 1,
		};

		private protected Mock<IEventService> _mockEventService;
		private protected Mock<IVenueService> _mockVenueService;
		private protected Mock<IEventPlaceService<EventSeatDto>> _mockEventSeatService;
		private protected Mock<IEventPlaceService<EventAreaDto>> _mockEventAreaService;
		private protected Mock<IUserService> _mockUserService;
		private protected Mock<ITicketService> _mockTicketService;

		private protected PurchaseController _controller;

		[SetUp]
		public void Setup()
		{
			_mockEventService = new Mock<IEventService>();
			_mockVenueService = new Mock<IVenueService>();
			_mockEventAreaService = new Mock<IEventPlaceService<EventAreaDto>>();
			_mockEventSeatService = new Mock<IEventPlaceService<EventSeatDto>>();
			_mockUserService = new Mock<IUserService>();
			_mockTicketService = new Mock<ITicketService>();

			_controller = new PurchaseController(_mockEventService.Object, _mockEventAreaService.Object,
				_mockVenueService.Object, _mockEventSeatService.Object, _mockUserService.Object,
				_mockTicketService.Object);
		}
	}

}
