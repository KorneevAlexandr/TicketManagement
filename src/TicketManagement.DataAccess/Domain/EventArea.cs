namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior areas, belonging to the event.
	/// Expands area.
	/// </summary>
	public class EventArea : Area
	{
		/// <summary>
		/// Shows affiliation of the area to the event.
		/// </summary>
		public int EventId { get; set; }

		/// <summary>
		/// Price the visit for event.
		/// </summary>
		public decimal Price { get; set; }
	}
}
