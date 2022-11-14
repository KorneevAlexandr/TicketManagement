using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Layouts;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.VenueAPI.Controllers
{
	/// <summary>
	/// Provides methods for getting, deleting, changing and creating layouts models.
	/// Available to a user with a role ModeratorVenue.
	/// </summary>
	[Authorize(Roles = "ModeratorVenue, ModeratorEvent")]
	[Route("[controller]")]
	[ApiController]
	public class LayoutManagementController : Controller
	{
		private readonly IVenueService _venueService;
		private readonly IServiceBase<LayoutDto> _layoutService;

		public LayoutManagementController(IVenueService venueService, IServiceBase<LayoutDto> layoutService)
		{
			_venueService = venueService;
			_layoutService = layoutService;
		}

		[HttpGet("Layouts")]
		public async Task<IActionResult> GetLayouts(int venueId)
		{
			var venuesNames = await FindAllVenueNames();

			if (venueId == 0)
			{
				var simpleModelView = new LayoutModel
				{
					InfoVenues = venuesNames,
					Layouts = new List<LayoutInfoModel>(),
				};
				return Json(simpleModelView);
			}

			var layouts = await _layoutService.GetAllAsync(venueId);
			var venue = await _venueService.GetAsync(venueId);

			var modelView = new LayoutModel
			{
				InfoVenues = venuesNames,

				Layouts = layouts.Select(layout => new LayoutInfoModel
				{
					Id = layout.Id,
					Name = layout.Name,
					Description = layout.Description,
					VenueName = venue.Name,
				}).ToList(),
			};

			return Json(modelView);
		}

		[HttpGet("Layout")]
		public async Task<IActionResult> GetLayout(int layoutId)
		{
			if (layoutId == 0)
			{
				return NotFound();
			}

			var layout = await _layoutService.GetAsync(layoutId);
			if (layout != null)
			{
				var model = await CreateLayoutDetails(layout);

				return Json(model);
			}

			return NotFound();
		}

		[HttpPost("Layout")]
		public async Task<IActionResult> CreateLayout([FromBody] CreateLayoutModel model)
		{
			if (model.VenueId <= 0)
			{
				return BadRequest();
			}

			var layout = new LayoutDto
			{
				Name = model.Name,
				Description = model.Description,
				VenueId = model.VenueId,
			};

			await _layoutService.AddAsync(layout);

			return Ok();
		}

		private async Task<LayoutFullInfoModel> CreateLayoutDetails(LayoutDto layout)
		{
			var venue = await _venueService.GetAsync(layout.VenueId);
			var layoutModel = new LayoutInfoModel
			{
				Name = layout.Name,
				VenueName = venue.Name,
				Description = layout.Description,
				Id = layout.Id,
				VenueId = venue.Id,
			};

			var model = new LayoutFullInfoModel
			{
				Layout = layoutModel,
			};

			return model;
		}

		[HttpDelete("Layout")]
		public async Task<IActionResult> DeleteLayout(int layoutId)
		{
			await _layoutService.DeleteAsync(layoutId);
			return Ok();
		}

		[HttpPut("Layout")]
		public async Task<IActionResult> UpdateLayout([FromBody] LayoutInfoModel model)
		{
			var layout = await _layoutService.GetAsync(model.Id);

			layout.Name = model.Name;
			layout.Description = model.Description;

			await _layoutService.UpdateAsync(layout);
			return Ok();
		}

		private async Task<List<VenueInfoModel>> FindAllVenueNames()
		{
			var venues = await _venueService.GetAllAsync();
			var venuesNames = venues.Select(venue => new VenueInfoModel
			{
				Id = venue.Id,
				Name = venue.Name,
			}).ToList();

			return venuesNames;
		}
	}
}
