using System;

namespace TicketManagement.ClientModels.Events
{
	/// <summary>
	/// Model describing event.
	/// </summary>
	public class EventModel
	{
		public int Id { get; set; }

		public string Description { get; set; }

		public string Name { get; set; }

		public DateTime DateTimeStart { get; set; }

		public DateTime DateTimeEnd { get; set; }

		public string Price { get; set; }

		public int Tickets { get; set; }

		public int LayoutId { get; set; }

		public string URL { get; set; }

		public int VenueId { get; set; }
	}
}
