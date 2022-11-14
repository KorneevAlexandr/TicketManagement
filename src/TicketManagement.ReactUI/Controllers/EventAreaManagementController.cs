using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Events;
using TicketManagement.ReactUI.ApiClient;

namespace TicketManagement.ReactUI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class EventAreaManagementController : ControllerBase
	{
		private readonly IEventRestClient _eventClient;

		public EventAreaManagementController(IEventRestClient eventClient)
		{
			_eventClient = eventClient;
		}

		[HttpGet("EventAreas/{id}")]
		public async Task<IActionResult> GetEventAreas(int id)
		{
			var eventAreas = await _eventClient.GetEventAreas(id, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(eventAreas);
		}

		[HttpDelete("EventArea/{id}")]
		public async Task<IActionResult> DeleteEventArea(int id)
		{
			await _eventClient.DeleteEventArea(id, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(id);
		}

		[HttpPut("EventArea")]
		public async Task<IActionResult> UpdateEventArea(EventAreaModel model)
		{
			await _eventClient.UpdateEventArea(model, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(model);
		}

		[HttpGet("EventSeats/{id}")]
		public async Task<IActionResult> GetEventSeats(int id)
		{
			var eventSeats = await _eventClient.GetEventSeats(id, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(eventSeats.Seats.AsEnumerable());
		}

		[HttpPut("EventSeat/{id}")]
		public async Task<IActionResult> ChangeEventSeatState(int id)
		{
			await _eventClient.UpdateEventSeatState(id, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(id);
		}

	}
}
