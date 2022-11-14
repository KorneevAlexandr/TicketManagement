namespace TicketManagement.ClientModels.Areas
{
	/// <summary>
	/// Model describing area.
	/// </summary>
	public class AreaInfoModel
	{
		public int Id { get; set; }

		public string Description { get; set; }

		public int CoordX { get; set; }

		public int CoordY { get; set; }

		public int Rows { get; set; }

		public int Numbers { get; set; }

		public int LayoutId { get; set; }

		public int SeatsCount { get; set; }
	}
}
