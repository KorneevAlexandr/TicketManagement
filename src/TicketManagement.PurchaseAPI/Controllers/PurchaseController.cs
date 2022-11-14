using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Events;
using TicketManagement.ClientModels.Tickets;

namespace TicketManagement.PurchaseAPI.Controllers
{
	/// <summary>
	/// Provides methods for getting and implementing event ticket models.
	/// Available to users with roles ModeratorEvent, User, ModeratorEvent.
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	[Authorize(Roles = "User, ModeratorVenue, ModeratorEvent")]
	public class PurchaseController : Controller
	{
		private readonly IEventService _eventService;
		private readonly IVenueService _venueService;
		private readonly IEventPlaceService<EventSeatDto> _eventSeatService;
		private readonly IEventPlaceService<EventAreaDto> _eventAreaService;
		private readonly IUserService _userService;
		private readonly ITicketService _ticketService;

		public PurchaseController(IEventService eventService, IEventPlaceService<EventAreaDto> eventAreaService, IVenueService venueService,
			IEventPlaceService<EventSeatDto> eventSeatService, IUserService userService, ITicketService ticketService)
		{
			_eventService = eventService;
			_venueService = venueService;
			_eventSeatService = eventSeatService;
			_eventAreaService = eventAreaService;
			_userService = userService;
			_ticketService = ticketService;
		}

		[AllowAnonymous]
		[HttpGet("EventTickets")]
		public async Task<IActionResult> GetEventTickets(int eventId)
		{
			var selectedEvent = await _eventService.GetAsync(eventId);
			var eventAreas = await _eventAreaService.GetPlacesByEventIdAsync(eventId);

			var eventSeats = await _eventSeatService.GetPlacesByEventIdAsync(selectedEvent.Id);

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
					Tickets = eventSeats.Count(x => x.State == SeatStateDto.Free),
				},

				EventAreas = eventAreas.Select(eventArea => new EventAreaModel
				{
					Id = eventArea.Id,
					Description = eventArea.Description,
					CoordX = eventArea.CoordX,
					CoordY = eventArea.CoordY,
					EventId = eventArea.EventId,
					Price = eventArea.Price.ToString(),
					Tickets = eventSeats.Count(eventSeat => eventSeat.EventAreaId == eventArea.Id && eventSeat.State == SeatStateDto.Free),
				}).ToList(),
			};

			var venue = await _venueService.GetAsync(selectedEvent.VenueId);
			model.Address = venue.Address;
			model.VenueName = venue.Name;
			model.Phone = venue.Phone;

			return Json(model);
		}

		[HttpGet("EventSeats")]
		public async Task<IActionResult> GetEventSeats(int eventAreaId)
		{
			var eventSeats = await _eventSeatService.GetPlacesAsync(eventAreaId);
			var eventArea = await _eventAreaService.GetAsync(eventAreaId);

			var model = new EventSeatModel
			{
				Seats = eventSeats.Select(seat => new EventSeatInfo
				{
					Id = seat.Id,
					Number = seat.Number,
					Row = seat.Row,
					Booked = seat.State == SeatStateDto.Booked,
				}).ToList(),

				AreaDescription = eventArea.Description,
				Price = eventArea.Price.ToString(),
			};

			return Json(model);
		}

		[HttpGet("Ticket")]
		public async Task<IActionResult> GetReadyTicket(int eventSeatId)
		{
			var user = await _userService.GetAsync(User.Identity.Name);
			var eventSeat = await _eventSeatService.GetAsync(eventSeatId);
			var eventArea = await _eventAreaService.GetAsync(eventSeat.EventAreaId);
			var evt = await _eventService.GetAsync(eventArea.EventId);
			var venue = await _venueService.GetAsync(evt.VenueId);

			var ticketModel = new TicketForBuyModel
			{
				Event = new EventModel
				{
					DateTimeStart = evt.DateTimeStart,
					DateTimeEnd = evt.DateTimeEnd,
					Name = evt.Name,
				},

				EventSeatId = eventSeat.Id,
				UserId = user.Id,
				UserEmail = user.Email,
				UserScore = user.Score,
				UserFullName = string.Concat(user.Surname, " ", user.Name),
				Row = eventSeat.Row,
				Number = eventSeat.Number,
				Price = Convert.ToDouble(eventArea.Price),
				VenueName = venue.Name,
				Address = venue.Address,
				AreaDescription = eventArea.Description,
			};

			return Json(ticketModel);
		}

		[HttpPost("Ticket")]
		public async Task<IActionResult> DealTicket(int eventSeatId)
		{
			var eventSeat = await _eventSeatService.GetAsync(eventSeatId);
			var eventArea = await _eventAreaService.GetAsync(eventSeat.EventAreaId);
			var evt = await _eventService.GetAsync(eventArea.EventId);

			await BookedEventSeat(eventSeat);
			var userId = await SubtractionOfCostForUser(Convert.ToDouble(eventArea.Price));

			var ticket = new TicketDto
			{
				DateTimePurchase = DateTime.Now,
				EventSeatId = eventSeatId,
				UserId = userId,
				EventName = evt.Name,
				Price = Convert.ToDouble(eventArea.Price),
			};

			await _ticketService.AddAsync(ticket);
			return Ok();
		}

		private async Task BookedEventSeat(EventSeatDto eventSeat)
		{
			eventSeat.State = SeatStateDto.Booked;
			await _eventSeatService.UpdateAsync(eventSeat);
		}

		private async Task<int> SubtractionOfCostForUser(double price)
		{
			var user = await _userService.GetAsync(User.Identity.Name);
			user.Score -= Convert.ToDouble(price);
			_userService.Update(user);
			return user.Id;
		}
	}
}
