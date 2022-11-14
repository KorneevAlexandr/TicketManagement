using System;

namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior ticket.
	/// </summary>
	public class Ticket
	{
		public int Id { get; set; }

		public double Price { get; set; }

		public string EventName { get; set; }

		public DateTime DateTimePurchase { get; set; }

		public int UserId { get; set; }

		public int EventSeatId { get; set; }
	}
}
