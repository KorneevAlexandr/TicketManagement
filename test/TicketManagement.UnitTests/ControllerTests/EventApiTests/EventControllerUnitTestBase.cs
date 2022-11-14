using Moq;
using NUnit.Framework;
using TicketManagement.EventAPI.Controllers;
using TicketManagement.EventAPI.Models;
using TicketManagement.EventAPI.Services.Interfaces;

namespace TicketManagement.UnitTests.ControllerTests.EventApiTests
{
	/// <summary>
	/// Basic setup for test controller classes EventAPI.
	/// </summary>
	public class EventControllerUnitTestBase
	{
		private protected Mock<IEventPlaceService<EventSeatDto>> _mockEventSeatService;
		private protected Mock<IEventPlaceService<EventAreaDto>> _mockEventAreaService;
		private protected Mock<IEventService> _mockEventService;
		private protected EventManagementController _eventController;
		private protected EventAreaManagementController _eventAreaController;
		private protected EventSeatManagementController _eventSeatController;

		[SetUp]
		public void Setup()
		{
			_mockEventSeatService = new Mock<IEventPlaceService<EventSeatDto>>();
			_mockEventAreaService = new Mock<IEventPlaceService<EventAreaDto>>();
			_mockEventService = new Mock<IEventService>();
			_eventController = new EventManagementController(_mockEventService.Object);
			_eventSeatController = new EventSeatManagementController(_mockEventSeatService.Object);
			_eventAreaController = new EventAreaManagementController(_mockEventAreaService.Object);
		}
	}
}
