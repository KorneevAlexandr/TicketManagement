using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TicketManagement.WebApplication.Clients;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides venue management operations.
	/// Only available to users of the ModeratorVenue role.
	/// </summary>
	[Authorize(Roles = "ModeratorVenue")]
	public class ModeratorVenueController : Controller
	{
		private readonly IVenueRestClient _venueClient;

		public ModeratorVenueController(IVenueRestClient venueClient)
		{
			_venueClient = venueClient;
		}
			
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var model = await _venueClient.GetVenues(HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(VenueModel venueModel)
		{
			await _venueClient.CreateVenue(venueModel, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Details(int? id)
		{
			var model = await _venueClient.GetVenue(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			var model = await _venueClient.GetVenue(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(VenueModel venueModel)
		{
			await _venueClient.UpdateVenue(venueModel, HttpContext.Request.Cookies["Token"]);
			return Redirect($"~/ModeratorVenue/Details?id={venueModel.Id}");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			var model = await _venueClient.GetVenue(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await _venueClient.DeleteVenue(id, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Index");
		}
	}
}
