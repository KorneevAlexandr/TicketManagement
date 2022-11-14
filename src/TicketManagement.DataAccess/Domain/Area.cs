namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior area.
	/// </summary>
	public class Area
	{
		/// <summary>
		/// Unique identifier area.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Shows affiliation of the area to the layout.
		/// </summary>
		public int LayoutId { get; set; }

		/// <summary>
		/// Description area.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Coordinate horizontal.
		/// </summary>
		public int CoordX { get; set; }

		/// <summary>
		/// Coordiname vertical.
		/// </summary>
		public int CoordY { get; set; }
	}
}
