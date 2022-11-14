using System;

namespace TicketManagement.ClientModels.Tickets
{
	/// <summary>
	/// Model describing Ticket.
	/// </summary>
	public class TicketViewModel
	{
		public DateTime DateTimePurchase { get; set; }

		public double Price { get; set; }

		public string EventName { get; set; }
	}
}
