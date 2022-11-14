using System.Collections.Generic;

namespace TicketManagement.ClientModels.Users
{
	/// <summary>
	/// Model descriping all Users for Admin.
	/// Provides the ability to use page navigation.
	/// </summary>
	public class UsersForAdminModel
	{
		public IEnumerable<UserModel> Users { get; set; }

		public int NumberPages { get; set; }

		public int NumberActivityPage { get; set; }

		public int[] RoleId { get; set; }
	}
}
