using System;
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
	/// Locates unit tests for the EventApi EventManagementController.
	/// </summary>
	public class EventManagementControllerTest : EventControllerUnitTestBase
	{
		[Test]
		public async Task GetLatestEvent_AnyEventCount_ReturnJsonResult()
		{
			// Arrange, Act
			var result = await _eventController.GetLatestEvents(1);

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

		[Test]
		public async Task GetLatestEvent_OneEvent_ReturnValidModel()
		{
			// Arrange
			var anyCount = 1;
			var expectedEvents = new List<EventDto>
			{
				new EventDto 
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
				},
			};
			var exptectedModel = expectedEvents.Select(evt => new EventModel
			{
				Id = evt.Id,
				DateTimeStart = evt.DateTimeStart,
				DateTimeEnd = evt.DateTimeEnd,
				Name = evt.Name,
				URL = evt.URL,
			}).ToList();

			_mockEventService.Setup(method => method.GetLatestAsync(anyCount))
				.ReturnsAsync(expectedEvents);

			// Act
			var jsonResult = await _eventController.GetLatestEvents(anyCount);

			// Assert
			var modelResult = (List<EventModel>)((JsonResult)jsonResult).Value;
			exptectedModel.Should().BeEquivalentTo(modelResult);
		}

		[Test]
		public async Task GetEvents_ReturnedType_ReturnJsonResult()
		{
			// Arrange 
			var venueId = 1;

			//  Act
			var result = await _eventController.GetEvents(venueId);

			// Assert
			result.Should().BeOfType<JsonResult>();
		}

		[Test]
		public async Task GetEvents_ReturnedModel_ReturnValidModel()
		{
			// Arrange 
			var venueId = 1;
			var expectedModel = new EventCollectionModel
			{
				Events = new List<EventModel>(),
			};

			// Act
			var jsonResult = await _eventController.GetEvents(venueId);

			// Assert
			var modelResult = (EventCollectionModel)((JsonResult)jsonResult).Value;
			expectedModel.Should().BeEquivalentTo(modelResult);
		}

		[Test]
		public async Task GetEvent_ExistEvent_ReturnValidModel()
		{
			// Arrange
			var selectedEvent = new EventDto
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
			var expectedModel = new EventFullModel
			{
				Event = new EventModel
				{
					Id = selectedEvent.Id,
					Name = selectedEvent.Name,
					Description = selectedEvent.Description,
					DateTimeStart = selectedEvent.DateTimeStart,
					LayoutId = selectedEvent.LayoutId,
					DateTimeEnd = selectedEvent.DateTimeEnd,
					URL = selectedEvent.URL,
					VenueId = 1,
				},
			};

			_mockEventService.Setup(method => method.GetAsync(selectedEvent.Id))
				.ReturnsAsync(selectedEvent);

			// Act
			var jsonResult = await _eventController.GetEvent(selectedEvent.Id);

			// Assert
			var modelResult = (EventFullModel)((JsonResult)jsonResult).Value;
			modelResult.Should().BeEquivalentTo(expectedModel);
		}

		[Test]
		public async Task DeleteEvent_IdDeletedEventNotExist_ReturnForbid()
		{
			// Arrange
			var falseId = 100;
			var events = new List<EventDto>
			{
				new EventDto { Id = 1 },
			};
			_mockEventService.Setup(method => method.GetAsync(falseId))
				.ReturnsAsync(events.FirstOrDefault(evt => evt.Id == falseId));

			// Act
			var result = await _eventController.DeleteEvent(falseId);

			// Assert
			result.Should().BeOfType<ForbidResult>();
		}

		[Test]
		public async Task CreateEvent_ValidModel_ReturnOkResult()
		{
			// Arrange
			var eventCreateModel = new EventTransportModel
			{
				Id = 1,
				DateTimeEnd = DateTime.Now.ToString(),
				DateTimeStart = DateTime.Now.ToString(),
				Description = "Good",
				Name = "Hey!",
				LayoutId = 1,
				Price = "100",
				URL = "my_path",
				VenueId = 1,
			};

			// Act
			var result = await _eventController.CreateEvent(eventCreateModel);

			// Assert
			result.Should().BeOfType<OkResult>();
		}

		[Test]
		public async Task UpdateEvent_ValidModel_ReturnOkResult()
		{
			// Arrange
			var eventUpdateModel = new EventTransportModel
			{
				Id = 1,
				DateTimeEnd = DateTime.Now.ToString(),
				DateTimeStart = DateTime.Now.ToString(),
				Description = "Good",
				Name = "Hey!",
				LayoutId = 1,
				Price = "100",
				URL = "my_path",
				VenueId = 1,
			};

			_mockEventService.Setup(method => method.GetAsync(eventUpdateModel.Id))
				.ReturnsAsync(new EventDto());

			// Act
			var result = await _eventController.UpdateEvent(eventUpdateModel);

			// Assert
			result.Should().BeOfType<OkResult>();
		}
	}
}
