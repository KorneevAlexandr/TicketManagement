using System.Linq;
using System.Collections.Generic;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection DTO-layouts in memory.
	/// Used by BL-services.
	/// </summary>
	public static class LayoutDtoData
	{
		/// <summary>
		/// Coolection layouts.
		/// </summary>
		public static List<LayoutDto> GetLayoutsDto => new List<LayoutDto>
		{
			new LayoutDto { Id = 1, Name = "Hol 2", VenueId = 1, Description = "Second layout" },
			new LayoutDto { Id = 2, Name = "Hol 1", VenueId = 1, Description = "First layout" },
		};

		/// <summary>
		/// Get layoutsDto by VenueId.
		/// </summary>
		/// <param name="id">VenueId.</param>
		/// <returns>LayoutsDto.</returns>
		public static List<LayoutDto> GetById(int id)
		{
			return GetLayoutsDto.Where(x => x.VenueId == id).ToList();
		}
	}
}
