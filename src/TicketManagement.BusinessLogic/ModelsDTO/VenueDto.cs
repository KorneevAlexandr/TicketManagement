namespace TicketManagement.BusinessLogic.ModelsDTO
{
	/// <summary>
	/// Entity venue for transfers between layers.
	/// </summary>
	public class VenueDto
	{
		/// <summary>
		/// Unique identifier venue.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Name venue.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Description venue.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Phone number venue.
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		/// Address venue.
		/// </summary>
		public string Address { get; set; }
	}
}
