using System;

namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior user.
	/// </summary>
	public class User
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public int Age { get; set; }

		public string Email { get; set; }

		public string Language { get; set; }

		public DateTimeOffset UTC { get; set; }

		public string Password { get; set; }

		public string Login { get; set; }

		public double Score { get; set; }
	}
}
