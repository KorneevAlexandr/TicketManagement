using System.Collections.Generic;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.ClientModels.Events
{
	/// <summary>
	/// Model containing events, venues.
	/// </summary>
	public class EventCollectionModel
	{
		public int Id { get; set; }
			
		public List<VenueInfoModel> InfoVenues { get; set; }

		public List<EventModel> Events { get; set; }
	}
}
