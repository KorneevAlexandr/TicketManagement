namespace TicketManagement.BusinessLogic.ModelsDTO
{
	/// <summary>
	/// Entity eventArea for transfers between layers.
	/// </summary>
	public class EventAreaDto : AreaDto
	{
		/// <summary>
		/// Shows affiliation of the area to the event.
		/// </summary>
		public int EventId { get; set; }

		/// <summary>
		/// Price the visit for event.
		/// </summary>
		public decimal Price { get; set; }
	}
}
