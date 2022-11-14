namespace TicketManagement.BusinessLogic.ModelsDTO
{
	/// <summary>
	/// Entity eventSeat for transfers between layers.
	/// </summary>
	public class EventSeatDto : SeatDto
	{
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
