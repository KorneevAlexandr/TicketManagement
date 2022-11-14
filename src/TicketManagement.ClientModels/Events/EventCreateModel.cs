using System;
using System.Collections.Generic;
using TicketManagement.ClientModels.Layouts;

namespace TicketManagement.ClientModels.Events
{
	/// <summary>
	/// Model describing event for created.
	/// </summary>
	public class EventCreateModel
	{
		public List<LayoutInfoModel> Layouts { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime DateTimeStart { get; set; }

		public TimeSpan TimeStart { get; set; }

		public DateTime DateTimeEnd { get; set; }

		public TimeSpan TimeEnd { get; set; }

		public string Price { get; set; }

		public string Phone { get; set; }

		public string Address { get; set; }

		public string VenueName { get; set; }

		public int VenueId { get; set; }

		public int LayoutId { get; set; }

		public string URL { get; set; }
	}
}
