using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.ClientModels.Events;
using TicketManagement.EventAPI.Models;
using TicketManagement.EventAPI.Services.Interfaces;

namespace TicketManagement.EventAPI.Controllers
{
	/// <summary>
	/// Provides methods for getting, deleting and changing eventArea models.
	/// Available to a user with a role ModeratorEvent.
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	[Authorize(Roles = "ModeratorEvent")]
	public class EventAreaManagementController : Controller
	{
		private readonly IEventPlaceService<EventAreaDto> _eventAreaService;

		public EventAreaManagementController(IEventPlaceService<EventAreaDto> eventAreaService)
		{
			_eventAreaService = eventAreaService;
		}

		[HttpGet("EventAreas")]
		public async Task<IActionResult> GetEventAreas(int eventId)
		{
			var eventAreas = await _eventAreaService.GetPlacesAsync(eventId);
			var model = eventAreas.Select(eventArea => new EventAreaModel
			{
				Id = eventArea.Id,
				Description = eventArea.Description,
				CoordX = eventArea.CoordX,
				CoordY = eventArea.CoordY,
				EventId = eventArea.EventId,
				Price = eventArea.Price.ToString(),
			}).ToList();

			return Json(model);
		}

		[HttpGet("EventArea")]
		public async Task<IActionResult> GetEventArea(int eventAreaId)
		{
			var updatedEventArea = await _eventAreaService.GetAsync(eventAreaId);
			if (updatedEventArea == null)
			{
				return Forbid();
			}

			var model = new EventAreaModel
			{
				Description = updatedEventArea.Description,
				CoordX = updatedEventArea.CoordX,
				CoordY = updatedEventArea.CoordY,
				EventId = updatedEventArea.EventId,
				Id = updatedEventArea.Id,
				Price = updatedEventArea.Price.ToString(),
			};

			return Json(model);
		}

		[HttpDelete("EventArea")]
		public async Task<IActionResult> DeleteEventArea(int eventAreaId)
		{
			var deletedEventArea = await _eventAreaService.GetAsync(eventAreaId);
			if (deletedEventArea == null)
			{
				return Forbid();
			}

			await _eventAreaService.DeleteAsync(deletedEventArea.Id);
			return Ok();
		}

		[HttpPut("EventArea")]
		public async Task<IActionResult> UpdateEventArea([FromBody] EventAreaModel model)
		{
			var updatedEventArea = await _eventAreaService.GetAsync(model.Id);
			if (updatedEventArea == null)
			{
				return Forbid();
			}

			updatedEventArea.CoordX = model.CoordX;
			updatedEventArea.CoordY = model.CoordY;
			updatedEventArea.Description = model.Description;
			updatedEventArea.Price = Convert.ToDecimal(model.Price);

			await _eventAreaService.UpdateAsync(updatedEventArea);
			return Ok();
		}
	}
}
