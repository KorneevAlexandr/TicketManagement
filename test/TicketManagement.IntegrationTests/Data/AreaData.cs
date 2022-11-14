using System.Linq;
using System.Collections.Generic;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.IntegrationTests.Data
{
	/// <summary>
	/// Keeps a collection areas in memory.
	/// For successful testing, this collection must match the data in the data source.
	/// </summary>
	public static class AreaData
	{
		/// <summary>
		/// Coolection areas.
		/// </summary>
		public static List<Area> GetAreas => new List<Area>
		{
			new Area { Id = 1, LayoutId = 1, CoordX = 1, CoordY = 1, Description = "Middle quality" },
			new Area { Id = 2, LayoutId = 1, CoordX = 2, CoordY = 2, Description = "High quality" },
			new Area { Id = 3, LayoutId = 2, CoordX = 1, CoordY = 1, Description = "Middle quality" },
		};

		/// <summary>
		/// Get areas bu LayoutId.
		/// </summary>
		/// <param name="id">LayoutId.</param>
		/// <returns>Areas.</returns>
		public static List<Area> GetById(int id)
		{
			return GetAreas.Where(s => s.LayoutId == id).ToList();
		}
	}
}
