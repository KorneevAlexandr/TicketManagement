namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior layout.
	/// </summary>
	public class Layout
	{
		/// <summary>
		/// Unique identifier layout.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The state of the layout at the venue.
		/// </summary>
		public int VenueId { get; set; }

		/// <summary>
		/// Description layout.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Name layout.
		/// </summary>
		public string Name { get; set; }
	}
}
