using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TicketManagement.ClientModels.Events;
using TicketManagement.EventAPI.Models;

namespace TicketManagement.UnitTests.ControllerTests.EventApiTests
{
	/// <summary>
	/// Locates unit tests for the EventApi EventSeatManagementController.
	/// </summary>
	public class EventSeatManagementControllerTest : EventControllerUnitTestBase
	{
		[Test]
		public async Task GetEventSeats_GettingSeatsModel_ReturnValidModel()
		{
			// Arrange
			var eventAreaId = 0;
			var eventSeats = new List<EventSeatDto>
			{
				new EventSeatDto
				{
					Id = 1, EventAreaId = eventAreaId, Number = 1, Row = 2, State = SeatStateDto.Free,
				},
				new EventSeatDto
				{
					Id = 2, EventAreaId = eventAreaId, Number = 2, Row = 2, State = SeatStateDto.Free,
				},
			};
			var expectedModel = new EventSeatModel
			{
				Seats = eventSeats.Select(seat => new EventSeatInfo
				{
					Id = seat.Id,
					Number = seat.Number,
					Row = seat.Row,
					Booked = seat.State == SeatStateDto.Booked,
				}).ToList(),
			};

			_mockEventSeatService.Setup(method => method.GetPlacesAsync(eventAreaId))
				.ReturnsAsync(eventSeats.Where(eventSeat => eventSeat.EventAreaId == eventAreaId));

			// Act
			var jsonResult = await _eventSeatController.GetEventSeats(eventAreaId);

			// Assert
			var modelResult = (EventSeatModel)((JsonResult)jsonResult).Value;
			expectedModel.Should().BeEquivalentTo(modelResult);
		}

		[Test]
		public async Task UpdateEventSeatState_TransferExistEventSeatId_ReturnOkResult()
		{
			// Arrange
			var eventSeat = new EventSeatDto
			{
				Id = 1,
				EventAreaId = 1,
				Number = 1,
				Row = 22,
				State = SeatStateDto.Free,
			};
			_mockEventSeatService.Setup(method => method.GetAsync(eventSeat.Id))
				.ReturnsAsync(eventSeat);

			// Act
			var result = await _eventSeatController.UpdateEventSeatState(eventSeat.Id);

			// Assert
			result.Should().BeOfType<OkResult>();
		}
	}
}
