using System.Linq;
using System.Collections.Generic;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection DTO-seats in memory.
	/// Used by BL-services.
	/// </summary>
	public static class SeatDtoData
	{
		/// <summary>
		/// Coolection seats.
		/// </summary>
		public static List<SeatDto> GetSeatsDto => new List<SeatDto>
		{
			new SeatDto { Id = 1, AreaId = 1, Row = 1, Number = 1 },
			new SeatDto { Id = 2, AreaId = 1, Row = 2, Number = 1 },
			new SeatDto { Id = 3, AreaId = 1, Row = 3, Number = 1 },
			new SeatDto { Id = 4, AreaId = 1, Row = 4, Number = 1 },
			new SeatDto { Id = 5, AreaId = 1, Row = 5, Number = 1 },
			new SeatDto { Id = 6, AreaId = 1, Row = 6, Number = 1 },
			new SeatDto { Id = 7, AreaId = 2, Row = 1, Number = 1 },
			new SeatDto { Id = 8, AreaId = 2, Row = 2, Number = 1 },
			new SeatDto { Id = 9, AreaId = 3, Row = 1, Number = 1 },
			new SeatDto { Id = 10, AreaId = 3, Row = 1, Number = 2 },
			new SeatDto { Id = 11, AreaId = 3, Row = 1, Number = 3 },
			new SeatDto { Id = 12, AreaId = 3, Row = 2, Number = 1 },
			new SeatDto { Id = 13, AreaId = 3, Row = 2, Number = 2 },
			new SeatDto { Id = 14, AreaId = 3, Row = 2, Number = 3 },
			new SeatDto { Id = 15, AreaId = 3, Row = 3, Number = 1 },
			new SeatDto { Id = 16, AreaId = 3, Row = 3, Number = 2 },
			new SeatDto { Id = 17, AreaId = 3, Row = 3, Number = 3 },
		};

		/// <summary>
		/// Get seatsDto by AreaId.
		/// </summary>
		/// <param name="id">AreaId.</param>
		/// <returns>SeatsDto.</returns>
		public static List<SeatDto> GetById(int id)
		{
			return GetSeatsDto.Where(x => x.AreaId == id).ToList();
		}
	}
}
