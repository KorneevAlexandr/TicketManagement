using System.Linq;
using System.Collections.Generic;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection DTO-areas in memory.
	/// Used by BL-services.
	/// </summary>
	public static class AreaDtoData
	{
		/// <summary>
		/// Coolection AreaDtos.
		/// </summary>
		public static List<AreaDto> GetAreasDto => new List<AreaDto>
		{
			new AreaDto { Id = 1, LayoutId = 1, CoordX = 1, CoordY = 1, Description = "Middle quality" },
			new AreaDto { Id = 2, LayoutId = 1, CoordX = 2, CoordY = 2, Description = "High quality" },
			new AreaDto { Id = 3, LayoutId = 2, CoordX = 1, CoordY = 1, Description = "Middle quality" },
		};

		/// <summary>
		/// Get areasDto bu LayoutId.
		/// </summary>
		/// <param name="id">LayoutId.</param>
		/// <returns>AreasDto.</returns>
		public static List<AreaDto> GetById(int id)
		{
			return GetAreasDto.Where(s => s.LayoutId == id).ToList();
		}
	}
}
