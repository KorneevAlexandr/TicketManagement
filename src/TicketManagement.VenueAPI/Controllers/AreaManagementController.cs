using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.ClientModels.Areas;

namespace TicketManagement.VenueAPI.Controllers
{
	/// <summary>
	/// Provides methods for getting, deleting, changing and creating areas models.
	/// Available to a user with a role ModeratorVenue.
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	[Authorize(Roles = "ModeratorVenue")]
	public class AreaManagementController : Controller
	{
		private readonly IServiceBase<AreaDto> _areaService;
		private readonly IServiceBase<SeatDto> _seatService;

		public AreaManagementController(IServiceBase<AreaDto> areaService, IServiceBase<SeatDto> seatService)
		{
			_areaService = areaService;
			_seatService = seatService;
		}

		[HttpGet("Areas")]
		public async Task<IActionResult> GetAreas(int layoutId)
		{
			var areas = await _areaService.GetAllAsync(layoutId);

			var areasModel = new List<AreaInfoModel>();
			foreach (var item in areas)
			{
				var seatsCount = await _seatService.GetAllAsync(item.Id);
				areasModel.Add(new AreaInfoModel
				{
					Id = item.Id,
					LayoutId = item.LayoutId,
					Description = item.Description,
					CoordX = item.CoordX,
					CoordY = item.CoordY,
					SeatsCount = seatsCount.Count(),
				});
			}

			return Json(areasModel);
		}

		[HttpGet("Area")]
		public async Task<IActionResult> GetArea(int areaId)
		{
			var area = await _areaService.GetAsync(areaId);

			var model = new AreaInfoModel
			{
				Id = area.Id,
				Description = area.Description,
				CoordX = area.CoordX,
				CoordY = area.CoordY,
			};

			return Json(model);
		}

		[HttpPost("Area")]
		public async Task<IActionResult> CreateArea([FromBody] AreaInfoModel model)
		{
			var area = new AreaDto
			{
				LayoutId = model.LayoutId,
				CoordX = model.CoordX,
				CoordY = model.CoordY,
				Description = model.Description,
			};

			await _areaService.AddAsync(area);
			var lastArea = await _areaService.GetAllAsync(model.LayoutId);
			var lastAreaId = lastArea.Last().Id;

			await GenerateSeats(model.Rows, model.Numbers, lastAreaId);
			return Ok();
		}

		/// <summary>
		/// Creates all seats. The number is rows multiplied by numbers.
		/// </summary>
		/// <param name="rows">All rows.</param>
		/// <param name="numbers">All numbers.</param>
		/// <param name="areaId">Area for new Seats.</param>
		/// <returns>Task(none).</returns>
		private async Task GenerateSeats(int rows, int numbers, int areaId)
		{
			for (int i = 1; i <= rows; i++)
			{
				for (int j = 1; j <= numbers; j++)
				{
					await _seatService.AddAsync(new SeatDto
					{
						AreaId = areaId,
						Row = i,
						Number = j,
					});
				}
			}
		}

		[HttpDelete("Area")]
		public async Task<IActionResult> DeleteArea(int areaId)
		{
			await _areaService.DeleteAsync(areaId);
			return Ok();
		}

		[HttpPut("Area")]
		public async Task<IActionResult> UpdateArea([FromBody] AreaInfoModel model)
		{
			var area = await _areaService.GetAsync(model.Id);

			if (area == null)
			{
				return BadRequest();
			}

			var areaDto = new AreaDto
			{
				Id = area.Id,
				LayoutId = area.LayoutId,
				Description = model.Description,
				CoordX = model.CoordX,
				CoordY = model.CoordY,
			};

			await _areaService.UpdateAsync(areaDto);
			return Ok();
		}
	}
}
