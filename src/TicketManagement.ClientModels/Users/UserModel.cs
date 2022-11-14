using TicketManagement.ClientModels.Accounts;

namespace TicketManagement.ClientModels.Users
{
	/// <summary>
	/// Model describing User.
	/// </summary>
	public class UserModel
	{
		public int Id { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public int Age { get; set; }

		public string Name { get; set; }

		public string Surname { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }

		public string RoleName { get; set; }

		public RoleState State { get; set; }

		public string Score { get; set; }
	}
}
