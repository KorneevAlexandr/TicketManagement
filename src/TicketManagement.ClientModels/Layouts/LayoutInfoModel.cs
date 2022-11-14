namespace TicketManagement.ClientModels.Layouts
{
	/// <summary>
	/// Model describing layout.
	/// </summary>
	public class LayoutInfoModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int VenueId { get; set; }

		public string VenueName { get; set; }
	}
}
