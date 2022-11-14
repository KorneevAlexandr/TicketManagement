using System.Collections.Generic;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.ClientModels.Layouts
{
	/// <summary>
	/// Model describing layout for created.
	/// </summary>
	public class CreateLayoutModel
	{
		public int VenueId { get; set; }

		public List<VenueInfoModel> InfoVenues { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
