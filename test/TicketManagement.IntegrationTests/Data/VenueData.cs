using System.Collections.Generic;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection venues in memory.
	/// For successful testing, this collection must match the data in the data source.
	/// </summary>
	public static class VenueData
	{
		/// <summary>
		/// Coolection venues.
		/// </summary>
		public static List<Venue> GetVenues => new List<Venue>
		{
			new Venue { Id = 1, Name = "Dance", Address = "Homel", Description = "Well dance", Phone = "+375291234567" },
		};

		/// <summary>
		/// Get Venue.
		/// </summary>
		/// <returns>Venue.</returns>
		public static Venue GetOne()
		{
			return GetVenues[0];
		}
	}
}
