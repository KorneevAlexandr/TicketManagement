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
	/// Provides methods for getting, changing eventSeat models.
	/// Available to a user with a role ModeratorEvent.
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	[Authorize(Roles = "ModeratorEvent")]
	public class EventSeatManagementController : Controller
	{
		private readonly IEventPlaceService<EventSeatDto> _eventSeatService;

		public EventSeatManagementController(IEventPlaceService<EventSeatDto> eventSeatService)
		{
			_eventSeatService = eventSeatService;
		}

		[HttpGet("EventSeats")]
		public async Task<IActionResult> GetEventSeats(int eventAreaId)
		{
			var eventSeats = await _eventSeatService.GetPlacesAsync(eventAreaId);

			var model = new EventSeatModel
			{
				Seats = eventSeats.Select(seat => new EventSeatInfo
				{
					Id = seat.Id,
					Number = seat.Number,
					Row = seat.Row,
					Booked = seat.State == SeatStateDto.Booked,
					EventAreaId = seat.EventAreaId,			
				}).ToList(),
			};

			return Json(model);
		}

		[HttpPut("EventSeatState")]
		public async Task<IActionResult> UpdateEventSeatState(int eventSeatId)
		{
			var eventSeat = await _eventSeatService.GetAsync(eventSeatId);
			if (eventSeat == null)
			{
				return Forbid();
			}

			var eventSeatDto = new EventSeatDto
			{
				Id = eventSeat.Id,
				AreaId = eventSeat.AreaId,
				EventAreaId = eventSeat.EventAreaId,
				Number = eventSeat.Number,
				Row = eventSeat.Row,
				State = eventSeat.State == SeatStateDto.Free ? SeatStateDto.Booked : SeatStateDto.Free,
			};

			await _eventSeatService.UpdateAsync(eventSeatDto);

			return Ok();
		}
	}
}
