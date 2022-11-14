using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.UserAPI.Models
{
	/// <summary>
	/// Describe user from data source.
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
