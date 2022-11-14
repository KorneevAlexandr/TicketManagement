namespace TicketManagement.BusinessLogic.ModelsDTO
{
	/// <summary>
	/// Entity seat for transfers between layers.
	/// </summary>
	public class SeatDto
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
