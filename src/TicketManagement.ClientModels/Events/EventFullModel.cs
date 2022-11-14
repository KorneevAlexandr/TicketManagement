using System.Collections.Generic;

namespace TicketManagement.ClientModels.Events
{
	/// <summary>
	/// Model describing event, with him eventAreas.
	/// </summary>
	public class EventFullModel
	{
		public EventModel Event { get; set; }

		public List<EventAreaModel> EventAreas { get; set; }

		public string VenueName { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public int FreeSeats { get; set; }
	}
}
