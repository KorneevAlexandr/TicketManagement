namespace TicketManagement.DataAccess.Domain
{
	/// <summary>
	/// Describes properties and behavior seat.
	/// </summary>
	public class Seat
	{
		/// <summary>
		/// Unique identifier seat.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Shows affiliation of the seat to the area.
		/// </summary>
		public int AreaId { get; set; }

		/// <summary>
		/// Row seat.
		/// </summary>
		public int Row { get; set; }

		/// <summary>
		/// Number seat in row.
		/// </summary>
		public int Number { get; set; }
	}
}
