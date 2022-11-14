namespace ThirdPartyEventEditor.Models
{
	/// <summary>
	/// Describes entity user for multi roles in application.
	/// </summary>
	public class User
	{
		public string Login { get; set; }
		
		public string Password { get; set; }

		public UserRole Role { get; set; }
	}
}