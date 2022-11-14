using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TicketManagement.WebApplication.Clients;
using TicketManagement.ClientModels.Layouts;
using TicketManagement.ClientModels.Areas;
using System.Linq;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides layout management operations.
	/// Only available to users of the ModeratorVenue role.
	/// </summary>
	[Authorize(Roles = "ModeratorVenue")]
	public class ModeratorLayoutController : Controller
	{
		private readonly IVenueRestClient _layoutClient;

		public ModeratorLayoutController(IVenueRestClient layoutClient)
		{
			_layoutClient = layoutClient;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return RedirectToAction("Layouts");
		}

		[HttpGet]
		public async Task<IActionResult> Layouts(int venueId)
		{
			var modelView = await _layoutClient.GetLayouts(venueId, HttpContext.Request.Cookies["Token"]);
			return View(modelView);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var venues = await _layoutClient.GetVenues(HttpContext.Request.Cookies["Token"]);
			var viewModel = new CreateLayoutModel
			{
				InfoVenues = venues.Select(venue => new VenueInfoModel
				{
					Id = venue.Id,
					Name = venue.Name,
				}).ToList(),
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateLayoutModel model)
		{
			await _layoutClient.CreateLayout(model, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var model = await _layoutClient.GetLayout(id, HttpContext.Request.Cookies["Token"]);
			model.Areas = await _layoutClient.GetAreas(id, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int? id)
		{
			await _layoutClient.DeleteLayout(id.Value, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			var layout = await _layoutClient.GetLayout(id, HttpContext.Request.Cookies["Token"]);
			var venue = await _layoutClient.GetVenue(layout.Layout.VenueId, HttpContext.Request.Cookies["Token"]);
			var model = new LayoutInfoModel
			{
				Name = layout.Layout.Name,
				VenueName = venue.Name,
				Description = layout.Layout.Description,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(LayoutInfoModel model)
		{
			await _layoutClient.UpdateLayout(model, HttpContext.Request.Cookies["Token"]);
			return Redirect($"~/ModeratorLayout/Details?id={model.Id}");
		}

		[HttpGet]
		public IActionResult CreateArea(int id)
		{
			var modelArea = new AreaInfoModel
			{
				LayoutId = id,
			};
			return View(modelArea);
		}

		[HttpPost]
		public async Task<IActionResult> CreateArea(AreaInfoModel model)
		{
			await _layoutClient.CreateArea(model, HttpContext.Request.Cookies["Token"]);
			return Redirect($"~/ModeratorLayout/Details?id={model.LayoutId}");
		}

		[HttpPost]
		public async Task<IActionResult> DeleteArea(int id)
		{
			await _layoutClient.DeleteArea(id, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> UpdateArea(int id)
		{
			var model = await _layoutClient.GetArea(id, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateArea(AreaInfoModel model)
		{
			await _layoutClient.UpdateArea(model, HttpContext.Request.Cookies["Token"]);
			return RedirectToAction("Index");
		}
	}
}