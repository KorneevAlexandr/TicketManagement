using System.Collections.Generic;
using TicketManagement.ClientModels.Areas;

namespace TicketManagement.ClientModels.Layouts
{
	/// <summary>
	/// Model describing layout, with him areas.
	/// </summary>
	public class LayoutFullInfoModel
	{
		public LayoutInfoModel Layout { get; set; }

		public List<AreaInfoModel> Areas { get; set; }
	}
}
