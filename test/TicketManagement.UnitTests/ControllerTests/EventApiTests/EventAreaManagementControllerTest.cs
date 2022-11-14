using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TicketManagement.ClientModels.Events;
using TicketManagement.EventAPI.Controllers;
using TicketManagement.EventAPI.Models;
using TicketManagement.EventAPI.Services.Interfaces;

namespace TicketManagement.UnitTests.ControllerTests.EventApiTests
{
	/// <summary>
	/// Locates unit tests for the EventApi EventAreaManagementController.
	/// </summary>
	public class EventAreaManagementControllerTest : EventControllerUnitTestBase
	{
		[Test]
		public async Task GetEventAreas_ValidEventId_ReturnEventAreaCollection()
		{
			// Arrange
			var anyId = 1;
			_mockEventAreaService.Setup(method => method.GetPlacesAsync(anyId))
				.ReturnsAsync(new List<EventAreaDto>());

			// Act
			var result = await _eventAreaController.GetEventAreas(anyId);

			// Assert
			var model = ((JsonResult)result).Value;
			model.Should().BeOfType<List<EventAreaModel>>();
		}

		[Test]
		public async Task GetEventArea_ValidEventId_ReturnEventArea()
		{
			// Arrange
			var eventId = 1;
			var eventArea = new EventAreaDto
			{
				Id = 1,
				CoordX = 1, 
				CoordY = 1, 
				Description = "Desc",
				EventId = 1,
				Price = 1,
			};
			var expectedEventArea = new EventAreaModel
			{
				Id = eventArea.Id,
				CoordX = eventArea.CoordX, 
				CoordY = eventArea.CoordY, 
				Description = eventArea.Description,
				Price = eventArea.Price.ToString(),
				EventId = eventArea.Id,
			};

			_mockEventAreaService.Setup(method => method.GetAsync(eventId))
				.ReturnsAsync(eventArea);

			// Act
			var result = await _eventAreaController.GetEventArea(eventId);

			// Assert
			var model = (EventAreaModel)((JsonResult)result).Value;
			model.Should().BeEquivalentTo(model);
		}

		[Test]
		public async Task DeleteEventArea_WhenEventAreaIdNotExist_ReturnForbid()
		{
			// Arrange
			var falseId = 100;
			var eventAreas = new List<EventAreaDto>
			{
				new EventAreaDto { Id = 1 },
			};
			_mockEventAreaService.Setup(method => method.GetAsync(falseId))
				.ReturnsAsync(eventAreas.FirstOrDefault(eventArea => eventArea.Id == falseId));

			// Act
			var result = await _eventAreaController.DeleteEventArea(falseId);

			// Assert
			result.Should().BeOfType<ForbidResult>();
		}

		[Test]
		public async Task UpdateEventArea_HttpPostModelValid_ReturnOkResult()
		{
			// Arrange
			var eventAreaModel = new EventAreaModel
			{
				Description = "EA",
				CoordX = 1,
				CoordY = 2,
				EventId = 1,
				Id = 1,
				Price = "10",
			};
			_mockEventAreaService.Setup(method => method.GetAsync(eventAreaModel.Id))
				.ReturnsAsync(new EventAreaDto());

			// Act
			var result = await _eventAreaController.UpdateEventArea(eventAreaModel);

			// Assert
			result.Should().BeOfType<OkResult>();
		}
	}
}
