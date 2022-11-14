using System;
using System.Collections.Generic;
using System.Text;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
	/// <summary>
	/// Entity ticket for transfers between layers.
	/// </summary>
	public class TicketDto
	{
		public int Id { get; set; }

		public double Price { get; set; }

		public string EventName { get; set; }

		public DateTime DateTimePurchase { get; set; }

		public int UserId { get; set; }

		public int EventSeatId { get; set; }
	}
}
