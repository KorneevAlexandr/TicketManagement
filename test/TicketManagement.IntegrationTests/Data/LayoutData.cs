using System.Linq;
using System.Collections.Generic;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection layouts in memory.
	/// For successful testing, this collection must match the data in the data source.
	/// </summary>
	public static class LayoutData
	{
		/// <summary>
		/// Coolection layouts.
		/// </summary>
		public static List<Layout> GetLayouts => new List<Layout>
		{
			new Layout { Id = 1, Name = "Hol 2", VenueId = 1, Description = "Second layout" },
			new Layout { Id = 2, Name = "Hol 1", VenueId = 1, Description = "First layout" },
		};

		/// <summary>
		/// Get layouts by VenueId.
		/// </summary>
		/// <param name="id">VenueId.</param>
		/// <returns>Layouts.</returns>
		public static List<Layout> GetById(int id)
		{
			return GetLayouts.Where(x => x.VenueId == id).ToList();
		}
	}
}
