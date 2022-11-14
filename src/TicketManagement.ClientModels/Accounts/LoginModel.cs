using System.ComponentModel.DataAnnotations;

namespace TicketManagement.ClientModels.Accounts
{
	/// <summary>
	/// Model for login user.
	/// </summary>
	public class LoginModel
	{
        [Required(ErrorMessage = "Not specified login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
	}
}
