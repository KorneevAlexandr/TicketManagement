using System.Collections.Generic;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection DTO-venues in memory.
	/// Used by BL-services.
	/// </summary>
	public static class VenueDtoData
	{
		/// <summary>
		/// Coolection venues.
		/// </summary>
		public static List<VenueDto> GetVenuesDto => new List<VenueDto>
		{
			new VenueDto { Id = 1, Name = "Dance", Address = "Homel", Description = "Well dance", Phone = "+375291234567" },
		};

		/// <summary>
		/// Get VenueDto.
		/// </summary>
		/// <returns>VenueDto.</returns>
		public static VenueDto GetOne()
		{
			return GetVenuesDto[0];
		}
	}
}
