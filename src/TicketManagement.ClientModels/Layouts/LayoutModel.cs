using System.Collections.Generic;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.ClientModels.Layouts
{
	/// <summary>
	/// Model describing all Layouts, with possible Venues.
	/// </summary>
	public class LayoutModel
	{
		public int Id { get; set; }

		public List<VenueInfoModel> InfoVenues { get; set; }

		public List<LayoutInfoModel> Layouts { get; set; }
	}
}
