using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.VenueAPI.Controllers;

namespace TicketManagement.UnitTests.ControllerTests.VenueApiTests
{
	/// <summary>
	/// Basic setup for test controller classes VenueAPI.
	/// </summary>
	public class VenueControllerUnitTestBase
	{
		private protected Mock<IVenueService> _mockVenueService;
		private protected Mock<IServiceBase<LayoutDto>> _mockLayoutService;
		private protected Mock<IServiceBase<AreaDto>> _mockAreaService;
		private protected Mock<IServiceBase<SeatDto>> _mockSeatService;

		private protected VenueManagementController _venueController;
		private protected LayoutManagementController _layoutController;
		private protected AreaManagementController _areaController;

		[SetUp]
		public void Setup()
		{
			_mockVenueService = new Mock<IVenueService>();
			_mockLayoutService = new Mock<IServiceBase<LayoutDto>>();
			_mockAreaService = new Mock<IServiceBase<AreaDto>>();
			_mockSeatService = new Mock<IServiceBase<SeatDto>>();
			
			_venueController = new VenueManagementController(_mockVenueService.Object);
			_layoutController = new LayoutManagementController(_mockVenueService.Object, _mockLayoutService.Object);
			_areaController = new AreaManagementController(_mockAreaService.Object, _mockSeatService.Object);
		}
	}
}
