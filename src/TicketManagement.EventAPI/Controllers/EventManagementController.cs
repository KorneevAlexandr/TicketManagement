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
	/// Provides methods for getting, deleting, changing and creating event models.
	/// Available to a user with a role ModeratorEvent.
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	public class EventManagementController : Controller
	{
		private readonly IEventService _eventService;

		public EventManagementController(IEventService eventService)
		{
			_eventService = eventService;
		}

		[AllowAnonymous]
		[HttpGet("LatestEvents")]
		public async Task<IActionResult> GetLatestEvents(int count)
		{
			var lastEvents = await _eventService.GetLatestAsync(count);

			var viewEvents = lastEvents.Select(evt => new EventModel
			{
				Id = evt.Id,
				DateTimeStart = evt.DateTimeStart,
				DateTimeEnd = evt.DateTimeEnd,
				Name = evt.Name,
				URL = evt.URL,
			}).ToList();

			return Json(viewEvents);
		}

		[AllowAnonymous]
		[HttpGet("EventsByNameAndDate")]
		public async Task<IActionResult> GetEventsByNameAndDate(string partEventName)
		{
			var startDateValid = DateTime.TryParse(HttpContext.Request.Headers["StartDate"], out DateTime startDate);
			startDate = startDateValid ? startDate : new DateTime(1800, 1, 1);
			var endDateValid = DateTime.TryParse(HttpContext.Request.Headers["EndDate"], out DateTime endDate);
			endDate = endDateValid && endDate > startDate ? endDate : DateTime.MaxValue;
			partEventName ??= string.Empty;

			var events = await _eventService.GetAllAsync(startDate, endDate, partEventName);

			var eventsModel = events.Select(evt => new EventModel
			{
				Id = evt.Id,
				Name = evt.Name,
				DateTimeStart = evt.DateTimeStart,
				DateTimeEnd = evt.DateTimeEnd,
				Description = evt.Description,
				URL = evt.URL,
			});

			return Json(eventsModel);
		}

		[Authorize(Roles = "ModeratorEvent")]
		[HttpGet("Events")]
		public async Task<IActionResult> GetEvents(int venueId)
		{
			var events = await _eventService.GetAllAsync(venueId);
			var model = new EventCollectionModel
			{
				Events = events.Select(evt => new EventModel
				{
					LayoutId = evt.LayoutId,
					Id = evt.Id,
					Name = evt.Name,
					DateTimeStart = evt.DateTimeStart,
					DateTimeEnd = evt.DateTimeEnd,
					Price = evt.Price.ToString(),
					Description = evt.Description,
					URL = evt.URL,
					VenueId = venueId,
				}).ToList(),
			};

			return Json(model);
		}

		[Authorize(Roles = "ModeratorEvent")]
		[HttpGet("Event")]
		public async Task<IActionResult> GetEvent(int eventId)
		{
			var selectedEvent = await _eventService.GetAsync(eventId);

			var model = new EventFullModel
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
					VenueId = selectedEvent.VenueId,
				},
			};

			return Json(model);
		}

		[Authorize(Roles = "ModeratorEvent")]
		[HttpDelete("Event")]
		public async Task<IActionResult> DeleteEvent(int eventId)
		{
			var deletedEvent = await _eventService.GetAsync(eventId);
			if (deletedEvent == null)
			{
				return Forbid();
			}

			await _eventService.DeleteAsync(deletedEvent.Id);
			return Ok();
		}

		[Authorize(Roles = "ModeratorEvent")]
		[HttpPost("Event")]
		public async Task<IActionResult> CreateEvent([FromBody] EventTransportModel model)
		{
			DateTime.TryParse(model.DateTimeStart, out DateTime dateStart);
			DateTime.TryParse(model.DateTimeEnd, out DateTime dateEnd);

			var createdEvent = new EventDto
			{
				Name = model.Name,
				Description = model.Description,
				DateTimeStart = dateStart,
				LayoutId = model.LayoutId,
				Price = Convert.ToDecimal(model.Price),
				DateTimeEnd = dateEnd,
				VenueId = model.VenueId,
				URL = model.URL,
			};

			await _eventService.AddAsync(createdEvent);
			return Ok();
		}

		[Authorize(Roles = "ModeratorEvent")]
		[HttpPut("Event")]
		public async Task<IActionResult> UpdateEvent([FromBody] EventTransportModel model)
		{
			DateTime.TryParse(model.DateTimeStart, out DateTime dateStart);
			DateTime.TryParse(model.DateTimeEnd, out DateTime dateEnd);
			var updatedEvent = await _eventService.GetAsync(model.Id);

			updatedEvent.Id = model.Id;
			updatedEvent.LayoutId = model.LayoutId;
			updatedEvent.DateTimeStart = dateStart;
			updatedEvent.DateTimeEnd = dateEnd;
			updatedEvent.Description = model.Description;
			updatedEvent.Name = model.Name;
			updatedEvent.URL = model.URL;

			await _eventService.UpdateAsync(updatedEvent);

			return Ok();
		}
	}
}
