using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Events;
using TicketManagement.ReactUI.ApiClient;

namespace TicketManagement.ReactUI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class EventManagementController : ControllerBase
	{
		private readonly IEventRestClient _eventClient;

		public EventManagementController(IEventRestClient eventClient)
		{
			_eventClient = eventClient;
		}

		[HttpGet("Events/{id}")]
		public async Task<IActionResult> Events(int id)
		{
			var events = await _eventClient.GetEvents(id, HttpContext.Request.Cookies["Token"]);

			return new JsonResult(events.Events);
		}

		[HttpGet("Event/{id}")]
		public async Task<IActionResult> GetEvent(int id)
		{
			var eventModel = await _eventClient.GetEvent(id, HttpContext.Request.Cookies["Token"]);

			return new JsonResult(eventModel.Event);
		}

		[HttpPost("Event")]
		public async Task<IActionResult> CreateEvent(EventCreateModel model)
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
			return new JsonResult(model);
		}

		[HttpPut("Event")]
		public async Task<IActionResult> EditEvent(EventCreateModel model)
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
			return new JsonResult(model);
		}

		[HttpDelete("Event/{id}")]
		public async Task<IActionResult> DeleteEvent(int id)
		{
			await _eventClient.DeleteEvent(id, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(id);
		}
	}
}
