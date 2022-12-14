using System;

namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior event.
	/// </summary>
	public class Event
	{
		/// <summary>
		/// Unique identifier event.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Description event.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Name event.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Shows affiliation of the event to the layout.
		/// </summary>
		public int LayoutId { get; set; }

		/// <summary>
		/// Date and time of the start of the event.
		/// </summary>
		public DateTime DateTimeStart { get; set; }

		/// <summary>
		/// Date and time of the end of the event.
		/// </summary>
		public DateTime DateTimeEnd { get; set; }

		/// <summary>
		/// Price for visit the event.
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// The state of the seats at the event.
		/// </summary>
		public SeatState State { get; set; }

		/// <summary>
		/// Venue id.
		/// </summary>
		public int VenueId { get; set; }

		/// <summary>
		/// Url image.
		/// </summary>
		public string URL { get; set; }
	}
}
