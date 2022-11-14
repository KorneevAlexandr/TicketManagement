namespace TicketManagement.EventAPI.Models
{
	/// <summary>
	/// Entity eventSeat for transfers between layers.
	/// </summary>
	public class EventSeatDto
	{
		/// <summary>
		/// Unique identifier seat.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Shows affiliation of the seat to the area.
		/// </summary>
		public int AreaId { get; set; }

		/// <summary>
		/// Row seat.
		/// </summary>
		public int Row { get; set; }

		/// <summary>
		/// Number seat in row.
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// Shows affiliation of the area to the event.
		/// </summary>
		public int EventAreaId { get; set; }

		/// <summary>
		/// The state of the seats at the event.
		/// </summary>
		public SeatStateDto State { get; set; }
	}
}
