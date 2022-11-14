using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Events;

namespace TicketManagement.UnitTests.ControllerTests.PurchaseApiTests
{
	/// <summary>
	/// Locates unit tests for the PurchaseApi PurchaseController.
	/// </summary>
	public class PurchaseControllerTest : PurchaseControllerUnitTestBase
	{
		[Test]
		public async Task EventTicket_SelectedEventId_ReturnEventTicketsModel()
		{
			// Arrange
			var testEventArea = new EventAreaDto
			{
				Description = "L",
				CoordX = 1,
				CoordY = 2,
				EventId = 1,
				Id = 1,
				Price = 100
			};
			var testEventSeat = new EventSeatDto
			{
				EventAreaId = 1,
				Id = 1,
				Number = 2,
				Row = 1,
				State = SeatStateDto.Free,
			};

			_mockEventService.Setup(method => method.GetAsync(_testEvent.Id))
				.ReturnsAsync(_testEvent);
			_mockEventAreaService.Setup(method => method.GetPlacesByEventIdAsync(_testEvent.Id))
				.ReturnsAsync(new List<EventAreaDto> { testEventArea });
			_mockEventSeatService.Setup(method => method.GetPlacesByEventIdAsync(_testEvent.Id))
				.ReturnsAsync(new List<EventSeatDto> { testEventSeat });
			_mockVenueService.Setup(method => method.GetAsync(_testEvent.VenueId))
				.ReturnsAsync(new VenueDto { Address = "T", Phone = "+123", Name = "T!", Id = 1 });

			// Act
			var result = await _controller.GetEventTickets(_testEvent.Id);

			// Assert
			var modelResult = (EventFullModel)((JsonResult)result).Value;
			modelResult.Event.Name.Should().BeEquivalentTo(_testEvent.Name);
		}

		[Test]
		public async Task EventSeats_EventAreaIdExist_ReturnEventSeatModel()
		{
			// Arrange
			var testEventArea = new EventAreaDto
			{
				Description = "L",
				CoordX = 1,
				CoordY = 2,
				EventId = 1,
				Id = 1,
				Price = 100
			};
			var testEventSeat = new EventSeatDto
			{
				EventAreaId = 1,
				Id = 1,
				Number = 2,
				Row = 1,
				State = SeatStateDto.Free,
			};
			var expectedModel = new EventSeatModel
			{
				AreaDescription = testEventArea.Description,
				Price = testEventArea.Price.ToString(),
				Seats = new List<EventSeatInfo>
				{
					new EventSeatInfo
					{
					Id = testEventSeat.Id,
					Number = testEventSeat.Number,
					Row = testEventSeat.Row,
					Booked = testEventSeat.State == SeatStateDto.Booked,
					},
				},
			};

			_mockEventAreaService.Setup(method => method.GetAsync(testEventArea.Id))
				.ReturnsAsync(testEventArea);
			_mockEventSeatService.Setup(method => method.GetPlacesAsync(testEventArea.Id))
				.ReturnsAsync(new List<EventSeatDto> { testEventSeat });
			
			// Act
			var jsonResult = await _controller.GetEventSeats(testEventArea.Id);

			// Assert
			var modelResult = (EventSeatModel)((JsonResult)jsonResult).Value;
			expectedModel.Should().BeEquivalentTo(modelResult);
		}
	}
}
