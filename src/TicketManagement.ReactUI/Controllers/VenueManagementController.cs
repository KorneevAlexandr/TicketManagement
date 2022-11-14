using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketManagement.ReactUI.ApiClient;

namespace TicketManagement.ReactUI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class VenueManagementController : ControllerBase	
	{
		private readonly IVenueRestClient _venueClient;

		public VenueManagementController(IVenueRestClient venueClient)
		{
			_venueClient = venueClient;
		}

		[HttpGet("Venues")]
		public async Task<IActionResult> GetVenues()
		{
			var model = await _venueClient.GetVenues(HttpContext.Request.Cookies["Token"]);
			return new JsonResult(model);
		}

		[HttpGet("Layouts")]
		public async Task<IActionResult> GetLayouts(int venueId)
		{
			var model = await _venueClient.GetLayouts(venueId, HttpContext.Request.Cookies["Token"]);
			return new JsonResult(model.Layouts);
		}
	}
}
