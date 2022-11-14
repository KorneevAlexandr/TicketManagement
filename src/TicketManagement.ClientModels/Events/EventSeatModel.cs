using System.Collections.Generic;
using System.Linq;

namespace TicketManagement.ClientModels.Events
{
	/// <summary>
	/// Model describing all EventSeat-s, allowing to display a map of places.
	/// </summary>
	public class EventSeatModel
	{
		private List<EventSeatInfo> _seats;

		public List<EventSeatInfo> Seats
		{
			get
			{
				return _seats;
			}
			set
			{
				_seats = value;

				Rows = _seats.Select(seat => seat.Row).Max();
				Numbers = _seats.Select(seat => seat.Number).Max();
			}
		}

		public string AreaDescription { get; set; }

		public string Price { get; set; }

		public int Rows { get; set; }

		public int Numbers { get; set; }

		public EventSeatInfo this[int row, int number]
		{
			get
			{
				return _seats.Find(s => s.Row == row && s.Number == number);
			}
		}
	}
}
