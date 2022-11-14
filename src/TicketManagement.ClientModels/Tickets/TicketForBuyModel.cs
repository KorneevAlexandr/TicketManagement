using TicketManagement.ClientModels.Events;

namespace TicketManagement.ClientModels.Tickets
{
	/// <summary>
	/// Model describing Ticket, displays purchase information.
	/// </summary>
	public class TicketForBuyModel
	{
		public EventModel Event { get; set; }

		public int EventSeatId { get; set; }

		public string AreaDescription { get; set; }

		public string VenueName { get; set; }

		public string Address { get; set; }

		public int UserId { get; set; }

		public double UserScore { get; set; }

		public string UserFullName { get; set; }

		public string UserEmail { get; set; }

		public double Price { get; set; }

		public int Row { get; set; }

		public int Number { get; set; }
	}
}
