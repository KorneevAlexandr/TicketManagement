using TicketManagement.DataAccess.Interfaces.Repositories;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.DataAccess.UnitOfWork
{
	/// <summary>
	/// Represents an encapsulation of all the repositories in a domain scope. 
	/// </summary>
	internal class UnitOfWork : IUnitOfWork
	{
		private readonly IBaseRepository<Seat> _seatRep;
		private readonly IBaseRepository<Area> _areaRep;
		private readonly IBaseRepository<Layout> _layoutRep;
		private readonly IVenueRepository _venueRep;
		private readonly IEventRepository _eventRep;
		private readonly IEventPlaceRepository<EventArea> _eventAreaRep;
		private readonly IEventPlaceRepository<EventSeat> _eventSeatRep;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
		/// </summary>
		/// <param name="seatRep">Seat repository.</param>
		/// <param name="areaRep">Area repository.</param>
		/// <param name="layoutRep">Layout repository.</param>
		/// <param name="venueRep">Venue repository.</param>
		/// <param name="eventRep">Event repository.</param>
		/// <param name="eventAreaRep">EventArea repository.</param>
		/// <param name="eventSeatRep">EventSeat repository.</param>
		public UnitOfWork(IBaseRepository<Seat> seatRep,
			IBaseRepository<Area> areaRep,
			IBaseRepository<Layout> layoutRep,
			IVenueRepository venueRep,
			IEventRepository eventRep,
			IEventPlaceRepository<EventArea> eventAreaRep,
			IEventPlaceRepository<EventSeat> eventSeatRep)
        {
			_seatRep = seatRep;
			_areaRep = areaRep;
			_layoutRep = layoutRep;
			_venueRep = venueRep;
			_eventRep = eventRep;
			_eventAreaRep = eventAreaRep;
			_eventSeatRep = eventSeatRep;
        }

		/// <summary>
		/// Repository for manipulating seat entity.
		/// </summary>
		public IBaseRepository<Seat> Seat
		{
			get => _seatRep;
		}

		/// <summary>
		/// Repository for manipulating area entity.
		/// </summary>
		public IBaseRepository<Area> Area
		{
			get => _areaRep;
		}

		/// <summary>
		/// Repository for manipulating layout entity.
		/// </summary>
		public IBaseRepository<Layout> Layout
		{
			get => _layoutRep;
		}

		/// <summary>
		/// Repository for manipulating venue entity.
		/// </summary>
		public IVenueRepository Venue
		{
			get => _venueRep;
		}

		/// <summary>
		/// Repository for manipulating event entity.
		/// </summary>
		public IEventRepository Event
		{
			get => _eventRep;
		}

		/// <summary>
		/// Repository for manipulating event-area entity.
		/// </summary>
		public IEventPlaceRepository<EventArea> EventArea
		{
			get => _eventAreaRep;
		}

		/// <summary>
		/// Repository for manipulating event-seat entity.
		/// </summary>
		public IEventPlaceRepository<EventSeat> EventSeat
		{
			get => _eventSeatRep;
		}
	}
}
