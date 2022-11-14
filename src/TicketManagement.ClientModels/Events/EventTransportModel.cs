using System;

namespace TicketManagement.ClientModels.Events
{
	public class EventTransportModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string DateTimeStart { get; set; }

		public string DateTimeEnd { get; set; }

		public string Price { get; set; }

		public int VenueId { get; set; }

		public int LayoutId { get; set; }

		public string URL { get; set; }
	}
}
