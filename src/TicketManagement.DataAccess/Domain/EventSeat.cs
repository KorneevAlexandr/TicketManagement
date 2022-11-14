namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior seats, belonging to the event.
	/// Expands seat.
	/// </summary>
	public class EventSeat : Seat
	{
		/// <summary>
		/// Shows affiliation of the seat to the area, belonging to the same event.
		/// </summary>
		public int EventAreaId { get; set; }

		/// <summary>
		/// The state of the seats at the event.
		/// </summary>
		public SeatState State { get; set; }
	}
}
