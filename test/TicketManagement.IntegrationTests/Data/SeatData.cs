using System.Linq;
using System.Collections.Generic;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection seats in memory.
	/// For successful testing, this collection must match the data in the data source.
	/// </summary>
	public static class SeatData
	{
		/// <summary>
		/// Coolection seats.
		/// </summary>
		public static List<Seat> GetSeats => new List<Seat>
		{
			new Seat { Id = 1, AreaId = 1, Row = 1, Number = 1 },
			new Seat { Id = 2, AreaId = 1, Row = 2, Number = 1 },
			new Seat { Id = 3, AreaId = 1, Row = 3, Number = 1 },
			new Seat { Id = 4, AreaId = 1, Row = 4, Number = 1 },
			new Seat { Id = 5, AreaId = 1, Row = 5, Number = 1 },
			new Seat { Id = 6, AreaId = 1, Row = 6, Number = 1 },
			new Seat { Id = 7, AreaId = 2, Row = 1, Number = 1 },
			new Seat { Id = 8, AreaId = 2, Row = 2, Number = 1 },
			new Seat { Id = 9, AreaId = 3, Row = 1, Number = 1 },
			new Seat { Id = 10, AreaId = 3, Row = 1, Number = 2 },
			new Seat { Id = 11, AreaId = 3, Row = 1, Number = 3 },
			new Seat { Id = 12, AreaId = 3, Row = 2, Number = 1 },
			new Seat { Id = 13, AreaId = 3, Row = 2, Number = 2 },
			new Seat { Id = 14, AreaId = 3, Row = 2, Number = 3 },
			new Seat { Id = 15, AreaId = 3, Row = 3, Number = 1 },
			new Seat { Id = 16, AreaId = 3, Row = 3, Number = 2 },
			new Seat { Id = 17, AreaId = 3, Row = 3, Number = 3 },
		};

		/// <summary>
		/// Get seats by AreaId.
		/// </summary>
		/// <param name="id">AreaId.</param>
		/// <returns>Seats.</returns>
		public static List<Seat> GetById(int id)
		{
			return GetSeats.Where(x => x.AreaId == id).ToList();
		}
	}
}
