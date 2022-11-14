namespace TicketManagement.EventAPI.Models
{
	/// <summary>
	/// Entity eventArea for transfers between layers.
	/// </summary>
	public class EventAreaDto
	{
		/// <summary>
		/// Shows affiliation of the area to the event.
		/// </summary>
		public int EventId { get; set; }

		/// <summary>
		/// Price the visit for event.
		/// </summary>
		public decimal Price { get; set; }

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
