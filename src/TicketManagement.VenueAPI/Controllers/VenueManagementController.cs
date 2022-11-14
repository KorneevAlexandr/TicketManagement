using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.VenueAPI.Controllers
{
	/// <summary>
	/// Provides methods for getting, deleting, changing and creating venues models.
	/// Available to a user with a role ModeratorVenue.
	/// </summary>
	[Authorize(Roles = "ModeratorVenue, ModeratorEvent")]
	[Route("[controller]")]
	[ApiController]
	public class VenueManagementController : Controller
	{
		private readonly IVenueService _venueService;

		public VenueManagementController(IVenueService venueService)
		{
			_venueService = venueService;
		}

		[HttpGet("Venues")]
		public async Task<IActionResult> GetVenues()
		{
			var venues = await _venueService.GetAllAsync();
			var venueModels = venues.Select(venue => new VenueModel
			{
				Name = venue.Name,
				Description = venue.Description,
				Address = venue.Address,
				Phone = venue.Phone,
				Id = venue.Id,
			}).ToList();

			return Json(venueModels);
		}

		[HttpGet("Venue")]
		public async Task<IActionResult> GetVenue(int venueId)
		{
			var model = await FindVenueAsync(venueId);
			if (model == null)
			{
				return NotFound();
			}

			return Json(model);
		}

		[HttpPost("Venue")]
		public async Task<IActionResult> CreateVenue([FromBody] VenueModel venueModel)
		{
			var venue = new VenueDto
			{
				Name = venueModel.Name,
				Description = venueModel.Description,
				Address = venueModel.Address,
				Phone = venueModel.Phone,
			};

			await _venueService.AddAsync(venue);

			return Ok();
		}

		[HttpDelete("Venue")]
		public async Task<IActionResult> DeleteVenue(int venueId)
		{
			await _venueService.DeleteAsync(venueId);
			return Ok();
		}

		[HttpPut("Venue")]
		public async Task<IActionResult> UpdateVenue([FromBody] VenueModel venueModel)
		{
			var venue = await _venueService.GetAsync(venueModel.Id);
			venue.Name = venueModel.Name;
			venue.Description = venueModel.Description;
			venue.Address = venueModel.Address;
			venue.Phone = venueModel.Phone;

			await _venueService.UpdateAsync(venue);
			return Ok();
		}

		private async Task<VenueModel> FindVenueAsync(int venueId)
		{
			var venue = await _venueService.GetAsync(venueId);
			if (venue == null)
			{
				return null;
			}

			var venueModel = new VenueModel
			{
				Id = venue.Id,
				Name = venue.Name,
				Description = venue.Description,
				Address = venue.Address,
				Phone = venue.Phone,
			};

			return venueModel;
		}
	}
}
