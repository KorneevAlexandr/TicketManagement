using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Events;
using TicketManagement.WebApplication.Clients;
using System.Linq;
using TicketManagement.ClientModels.Layouts;
using TicketManagement.ClientModels.Venues;
using System.Collections.Generic;
using Microsoft.FeatureManagement.Mvc;
using TicketManagement.WebApplication.Settings;
using System;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides event management operations.
	/// Only available to users of the ModeratorEvent role.
	/// </summary>
	[FeatureGate(FeatureFlags.UseOnlyMvc)]
	[Authorize(Roles = "ModeratorEvent")]
	public class ModeratorEventController : Controller
	{
		private readonly IEventRestClient _eventClient;
		private readonly IVenueRestClient _venueClient;

		public ModeratorEventController(IEventRestClient eventClient, IVenueRestClient venueClient)
		{
			_eventClient = eventClient;
			_venueClient = venueClient;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return RedirectToAction("Events");
		}

		[HttpGet]
		public async Task<IActionResult> Events(int? id)
		{
			var venues = await _venueClient.GetVenues(HttpContext.Request.Cookies["Token"]);
			if (id == null)
			{
				var simpleModel = new EventCollectionModel
				{
					Events = new List<EventModel>(),
					InfoVenues = venues.Select(venue => new VenueInfoModel
					{
						Id = venue.Id,
						Name = venue.Name,
					}).ToList(),
				};
				return View(simpleModel);
			}

			var model = await _eventClient.GetEvents(id.Value, HttpContext.Request.Cookies["Token"]);
			model.InfoVenues = venues.Select(venue => new VenueInfoModel
			{
				Id = venue.Id,
				Name = venue.Name,
			}).ToList();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var venue = await _venueClient.GetVenue(id.Value, HttpContext.Request.Cookies["Token"]);
			var layouts = await _venueClient.GetLayouts(id.Value, HttpContext.Request.Cookies["Token"]);

			var viewModel = new EventCreateModel
			{
				Layouts = layouts.Layouts.Select(layout => new LayoutInfoModel
				{
					Id = layout.Id,
					Name = layout.Name,
					Description = layout.Description,
					VenueId = venue.Id,
					VenueName = venue.Name,
				}).ToList(),
				VenueId = venue.Id,
				VenueName = venue.Name,
			};
			
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Create(EventCreateModel model)
		{
			model.DateTimeStart += model.TimeStart;
			model.DateTimeEnd += model.TimeEnd;
			try
			{
				var transportModel = new EventTransportModel
				{
					Name = model.Name,
					Description = model.Description,
					DateTimeStart = model.DateTimeStart.ToString(),
					DateTimeEnd = model.DateTimeEnd.ToString(),
					LayoutId = model.LayoutId,
					Price = model.Price,
					URL = model.URL,
					VenueId = model.VenueId,
				};
				await _eventClient.CreateEvent(transportModel, HttpContext.Request.Cookies["Token"]);
			}
			catch
			{
				return RedirectToAction("InvalidEventDate");
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var eventAreas = await _eventClient.GetEventAreas(id.Value, HttpContext.Request.Cookies["Token"]);
			var model = await _eventClient.GetEvent(id.Value, HttpContext.Request.Cookies["Token"]);
			model.EventAreas = eventAreas.ToList();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			try
			{
				await _eventClient.DeleteEvent(id.Value, HttpContext.Request.Cookies["Token"]);
			}
			catch
			{
				return RedirectToAction("InvalidDelete");
			}

			return Index();
		}

		public IActionResult InvalidDelete()
		{
			return View();
		}

		public IActionResult InvalidEventDate()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var updatedEvent = await _eventClient.GetEvent(id.Value, HttpContext.Request.Cookies["Token"]);
			var layoutModel = await _venueClient.GetLayouts(updatedEvent.Event.VenueId, HttpContext.Request.Cookies["Token"]);

			var viewModel = new EventCreateModel
			{
				Id = updatedEvent.Event.Id,
				LayoutId = updatedEvent.Event.LayoutId,
				DateTimeStart = updatedEvent.Event.DateTimeStart,
				DateTimeEnd = updatedEvent.Event.DateTimeEnd,
				Description = updatedEvent.Event.Description,
				Name = updatedEvent.Event.Name,
				URL = updatedEvent.Event.URL,
				TimeStart = updatedEvent.Event.DateTimeStart.TimeOfDay,
				TimeEnd = updatedEvent.Event.DateTimeEnd.TimeOfDay,

				Layouts = layoutModel.Layouts.Select(layout => new LayoutInfoModel
				{
					Id = layout.Id,
					Name = layout.Name,
				}).ToList(),
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Update(EventCreateModel model)
		{
			model.DateTimeStart += model.TimeStart;
			model.DateTimeEnd += model.TimeEnd;
			try
			{
				var transportModel = new EventTransportModel
				{
					Id = model.Id,
					Name = model.Name,
					Description = model.Description,
					DateTimeStart = model.DateTimeStart.ToString(),
					DateTimeEnd = model.DateTimeEnd.ToString(),
					LayoutId = model.LayoutId,
					Price = model.Price,
					URL = model.URL,
					VenueId = model.VenueId,
				};
				await _eventClient.UpdateEvent(transportModel, HttpContext.Request.Cookies["Token"]);
			}
			catch
			{
				return RedirectToAction("InvalidEventDate");
			}
			return Index();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteEventArea(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			await _eventClient.DeleteEventArea(id.Value, HttpContext.Request.Cookies["Token"]);
			return Index();
		}

		[HttpGet]
		public async Task<IActionResult> UpdateEventArea(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _eventClient.GetEventArea(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateEventArea(EventAreaModel model)
		{
			await _eventClient.UpdateEventArea(model, HttpContext.Request.Cookies["Token"]);
			return Index();
		}

		[HttpGet]
		public async Task<IActionResult> Seats(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _eventClient.GetEventSeats(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ChangeSeatState(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			await _eventClient.UpdateEventSeatState(id.Value, HttpContext.Request.Cookies["Token"]);
			return Index();
		}
	}
}
