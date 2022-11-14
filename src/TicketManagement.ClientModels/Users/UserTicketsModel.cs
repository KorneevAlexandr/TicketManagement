using System.Collections.Generic;
using TicketManagement.ClientModels.Tickets;

namespace TicketManagement.ClientModels.Users
{
	/// <summary>
	/// Model describing User, with him bought tickets.
	/// </summary>
	public class UserTicketsModel
	{
		public UserModel User { get; set; }

		public List<TicketViewModel> Tickets { get; set; }
	}
}
