namespace TicketManagement.ClientModels.Accounts
{
	/// <summary>
	/// Describing user roles.
	/// Value filed this enum match roles in database.
	/// </summary>
	public enum RoleState
	{
		Admin = 1,
		User,
		ModeratorVenue,
		ModeratorEvent,
	}
}
