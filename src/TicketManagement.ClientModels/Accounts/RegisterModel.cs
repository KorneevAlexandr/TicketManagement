using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.ClientModels.Accounts
{
    /// <summary>
    /// Model for register user.
    /// </summary>
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name not specified")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname not specified")]
        public string Surname { get; set; }

        [Range(1, 120, ErrorMessage = "Invalid age value")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email not specified")]
        [EmailAddress(ErrorMessage = "Email entered incorrectly")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login not specified")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password entered incorrectly")]
        public string ConfirmPassword { get; set; }
    }
}
