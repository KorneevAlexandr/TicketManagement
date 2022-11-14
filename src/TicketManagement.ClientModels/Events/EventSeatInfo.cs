namespace TicketManagement.ClientModels.Events
{
	/// <summary>
	/// Model describing EventSeat.
	/// </summary>
	public class EventSeatInfo
	{
		public int Id { get; set; }

		public int Number { get; set; }

		public int Row { get; set; }

		public bool Booked { get; set; }

		public int EventAreaId { get; set; }
	}
}
