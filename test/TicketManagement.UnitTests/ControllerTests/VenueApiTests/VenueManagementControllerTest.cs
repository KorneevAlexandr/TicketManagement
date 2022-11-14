using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Venues;
using TicketManagement.VenueAPI.Controllers;

namespace TicketManagement.UnitTests.ControllerTests.VenueApiTests
{
	/// <summary>
	/// Locates unit tests for the VenueApi VenueManagementController.
	/// </summary>
	public class VenueManagementControllerTest : VenueControllerUnitTestBase
	{
		[Test]
		public async Task GetVenues_GetType_JsonResultType()
		{
			// Arrange, Act
			var result = await _venueController.GetVenues();

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

		[Test]
		public async Task GetVenues_GetSerializeModel_CorrectVenueModel()
		{
			// Arrange
			var expectedVenues = new List<VenueDto>
			{
				new VenueDto { Id = 1, Description = "1", Address = "1", Name = "1", Phone = "1" },
				new VenueDto { Id = 2, Description = "2", Address = "2", Name = "2", Phone = "2" },
			};
			_mockVenueService.Setup(method => method.GetAllAsync()).ReturnsAsync(expectedVenues);
			var jsonResult = await _venueController.GetVenues();

			// Act
			var modelResult = (List<VenueModel>)((JsonResult)jsonResult).Value;

			// Assert
			modelResult.Should().BeEquivalentTo(expectedVenues);
		}

		[Test]
		public async Task GetVenue_ForOneVenue_ReturnVenueModel()
		{
			// Arrange
			var expectedVenue = new VenueDto { Id = 1, Name = "HS", Description = "DS", Address = "Ad", Phone = "+78879" };
			_mockVenueService.Setup(method => method.GetAsync(expectedVenue.Id)).ReturnsAsync(expectedVenue);
			var jsonResult = await _venueController.GetVenue(expectedVenue.Id);

			// Act
			var modelResult = (VenueModel)((JsonResult)jsonResult).Value;

			// Assert
			expectedVenue.Should().BeEquivalentTo(modelResult);
		}

		[Test]
		public async Task CreateVenue_WhenVenueValid_AddingVenue()
		{
			// Arrange
			var venues = new List<VenueDto>();
			var expectedVenue = new VenueDto { Id = 1, Name = "HS", Description = "DS", Address = "Ad", Phone = "+78879" };
			var expectedVenueModel = new VenueModel { Id = 1, Name = "HS", Description = "DS", Address = "Ad", Phone = "+78879" };
			_mockVenueService.Setup(method => method.AddAsync(expectedVenue))
				.Callback<VenueDto>(venueModel => venues.Add(expectedVenue));
			_mockVenueService.Setup(method => method.GetAllAsync()).ReturnsAsync(venues);

			// Act
			await _venueController.CreateVenue(expectedVenueModel);

			// Assert
			var jsonResult = await _venueController.GetVenues();
			var modelResult = (List<VenueModel>)((JsonResult)jsonResult).Value;
			venues.Should().BeEquivalentTo(modelResult);
		}

		[Test]
		public async Task UpdateVenue_WhenVenueModelValid_ReturnUpdatedVenue()
		{
			// Arrange
			var venues = new List<VenueDto> { new VenueDto { Id = 1, Name = "HS", Description = "DS", Address = "Ad", Phone = "+78879" } };
			var expectedVenue = new VenueDto { Id = 1, Name = "TT", Description = "TT", Address = "TT", Phone = "+111" };
			var expectedVenueModel = new VenueModel { Id = 1, Name = "TT", Description = "TT", Address = "TT", Phone = "+111" };
			_mockVenueService.Setup(method => method.UpdateAsync(expectedVenue))
				.Callback<VenueDto>(venueModel =>
                {
                    venues.Clear();
					venues.Add(expectedVenue);
                });
			_mockVenueService.Setup(method => method.GetAllAsync()).ReturnsAsync(venues);
			_mockVenueService.Setup(method => method.GetAsync(expectedVenue.Id)).ReturnsAsync(venues.FirstOrDefault());

			// Act
			await _venueController.UpdateVenue(expectedVenueModel);

			// Assert
			var jsonResult = await _venueController.GetVenues();
			var modelResult = (List<VenueModel>)((JsonResult)jsonResult).Value;
			modelResult.Should().BeEquivalentTo(new List<VenueDto> { expectedVenue });
		}
	}
}
