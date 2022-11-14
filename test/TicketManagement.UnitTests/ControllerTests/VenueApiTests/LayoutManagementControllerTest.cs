using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Layouts;
using TicketManagement.ClientModels.Venues;
using TicketManagement.VenueAPI.Controllers;

namespace TicketManagement.UnitTests.ControllerTests.VenueApiTests
{
	/// <summary>
	/// Locates unit tests for the VenueApi LayoutManagementController.
	/// </summary>
	public class LayoutManagementControllerTest : VenueControllerUnitTestBase
	{
		[Test]
		public async Task GetLayouts_HttpGet_ReturnJsonResult()
		{
			// Arrange
			int venueId = 1;
			_mockLayoutService.Setup(method => method.GetAllAsync(venueId))
				.ReturnsAsync(new List<LayoutDto>());

			// Act
			var result = await _layoutController.GetLayouts(venueId);

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

		[Test]
		public async Task GetLayouts_WithoutSelecteVenue_ReturnCollectionLayoutModels()
		{
			// Arrange
			var emptyVenueId = 0;
			var venueNames = new List<VenueDto>
			{
				new VenueDto { Id = 1, Description = "Good", Name = "Alex", Address = "Gomel", Phone = "+123" },
				new VenueDto { Id = 2, Description = "Bad", Name = "Maks", Address = "Mig", Phone = "+456s" },
			};
			var expectedModel = new LayoutModel
			{
				Layouts = new List<LayoutInfoModel>(),
				InfoVenues = venueNames.Select(venue => new VenueInfoModel
				{
					Id = venue.Id,
					Name = venue.Name,
				}).ToList(),
			};
			_mockVenueService.Setup(method => method.GetAllAsync()).ReturnsAsync(venueNames);

			// Act 
			var jsonResult = await _layoutController.GetLayouts(emptyVenueId);

			// Arrange
			var result = (LayoutModel)((JsonResult)jsonResult).Value;
			expectedModel.Should().BeEquivalentTo(result);
		}

		[Test]
		public async Task GetLayouts_WithOrderModel_ReturnLayoutModels()
		{
			// Arrange
			var venueNames = new List<VenueDto>
			{
				new VenueDto { Id = 1, Description = "Good", Name = "Alex", Address = "Gomel", Phone = "+123" },
				new VenueDto { Id = 2, Description = "Bad", Name = "Maks", Address = "Mig", Phone = "+456s" },
			};
			var layouts = new List<LayoutDto>
			{
				new LayoutDto { Id = 1, VenueId = 1, Name = "1", Description = "1" }, 
				new LayoutDto { Id = 2, VenueId = 1, Name = "2", Description = "2" }, 
				new LayoutDto { Id = 3, VenueId = 2, Name = "3", Description = "3" }, 
			};
			var enteringModel = new LayoutModel
			{
				Id = 1,
				Layouts = new List<LayoutInfoModel>(),
				InfoVenues = venueNames.Select(venue => new VenueInfoModel
				{ 
					Id = venue.Id,
					Name = venue.Name,
				}).ToList(),
			};
			var expectedModel = new LayoutModel
			{
				Layouts = layouts.Where(layout => layout.VenueId == enteringModel.Id)
					.Select(model => new LayoutInfoModel
					{
						Id = model.Id,
						Description = model.Description,
						Name = model.Name,
						VenueName = venueNames.FirstOrDefault().Name,
					}).ToList(),
				InfoVenues = venueNames.Select(venue => new VenueInfoModel
				{
					Id = venue.Id,
					Name = venue.Name,
				}).ToList(),
			};

			_mockVenueService.Setup(method => method.GetAllAsync()).ReturnsAsync(venueNames);
			_mockVenueService.Setup(method => method.GetAsync(enteringModel.Id))
				.ReturnsAsync(venueNames.FirstOrDefault(venue => venue.Id == enteringModel.Id));
			_mockLayoutService.Setup(method => method.GetAllAsync(enteringModel.Id))
				.ReturnsAsync(layouts.Where(layout => layout.VenueId == enteringModel.Id));

			// Act
			var jsonResult = await _layoutController.GetLayouts(enteringModel.Id);

			// Assert
			var result = (LayoutModel)((JsonResult)jsonResult).Value;
			expectedModel.Should().BeEquivalentTo(result);
		}

		[Test]
		public async Task CreateLayout_ModelNotValid_ReturnBadRequest()
		{
			// Arrange
			var layoutModel = new CreateLayoutModel
			{
				Name = "123",
				Description = "Top",
				VenueId = 0,
			};

			// Act
			var result = await _layoutController.CreateLayout(layoutModel);

			// Arrange
			result.Should().BeOfType<BadRequestResult>();
		}

		[Test]
		public async Task GetLayout_IdNotValid_ReturnNotFound()
		{
			// Arrange
			var badId = 0;

			// Act
			var result = await _layoutController.GetLayout(badId);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[Test]
		public async Task UpdateLayout_ModelValid_SuccessUpdate()
		{
			// Arrange
			var startedLayout = new LayoutDto { Id = 1, Name = "Hello, world!" };
			var expectedLayout = new LayoutDto { Id = 1, VenueId = 1, Description = "Desc", Name = "Good buy!" };
			var updatedLayoutModel = new LayoutInfoModel { Id = 1, VenueId = 1, Description = "Desc", Name = "Good buy!" };

			_mockLayoutService.Setup(method => method.GetAsync(It.IsAny<int>()))
				.ReturnsAsync(startedLayout);
			_mockLayoutService.Setup(method => method.UpdateAsync(It.IsAny<LayoutDto>()))
				.Callback<LayoutDto>(layout =>
				{
					startedLayout.Name = updatedLayoutModel.Name;
					startedLayout.Description = updatedLayoutModel.Description;
					startedLayout.VenueId = updatedLayoutModel.VenueId;
				});

			// Act
			await _layoutController.UpdateLayout(updatedLayoutModel);

			// Assert
			expectedLayout.Should().BeEquivalentTo(startedLayout);
		}
	}
}
