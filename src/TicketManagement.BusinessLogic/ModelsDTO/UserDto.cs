using System;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
	/// <summary>
	/// Entity user for transfers between layers.
	/// </summary>
	public class UserDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public int Age { get; set; }

		public string Language { get; set; }

		public string Email { get; set; }

		public DateTimeOffset UTC { get; set; }

		public string Password { get; set; }

		public string Login { get; set; }

		public double Score { get; set; }

		public string RoleName { get; set; }
	}
}
