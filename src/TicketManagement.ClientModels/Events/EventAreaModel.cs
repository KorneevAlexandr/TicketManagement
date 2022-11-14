namespace TicketManagement.ClientModels.Events
{
	/// <summary>
	/// Model describing eventArea.
	/// </summary>
	public class EventAreaModel
	{
		public int Id { get; set; }

		public int EventId { get; set; }

		public string Description { get; set; }

		public int CoordX { get; set; }

		public int CoordY { get; set; }

		public string Price { get; set; }

		public int Tickets { get; set; }
	}
}
