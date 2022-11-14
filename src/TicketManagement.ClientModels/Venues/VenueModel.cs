using System.ComponentModel.DataAnnotations;

namespace TicketManagement.ClientModels.Venues
{
	/// <summary>
	/// Describe venue model for view and transient.
	/// </summary>
	public class VenueModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name not be empty")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Description not be empty")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Address not be empty")]
		public string Address { get; set; }

		[Phone]
		public string Phone { get; set; }
	}
}
