using TicketManagement.DataAccess.Interfaces.Repositories;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.DataAccess.Interfaces.UnitOfWork
{
	/// <summary>
	/// Represents an encapsulation of all the repositories in a domain scope. 
	/// Ensures that each repository uses the same strategy and data context.
	/// </summary>
	public interface IUnitOfWork
	{
		/// <summary>
		/// Repository for manipulating seat entity.
		/// </summary>
		IBaseRepository<Seat> Seat { get; }

		/// <summary>
		/// Repository for manipulating area entity.
		/// </summary>
		IBaseRepository<Area> Area { get; }

		/// <summary>
		/// Repository for manipulating layout entity.
		/// </summary>
		IBaseRepository<Layout> Layout { get; }

		/// <summary>
		/// Repository for manipulating venue entity.
		/// </summary>
		IVenueRepository Venue { get; }

		/// <summary>
		/// Repository for manipulating event entity.
		/// </summary>
		IEventRepository Event { get; }

		/// <summary>
		/// Repository for manipulating event-area entity.
		/// </summary>
		IEventPlaceRepository<EventArea> EventArea { get; }

		/// <summary>
		/// Repository for manipulating event-seat entity.
		/// </summary>
		IEventPlaceRepository<EventSeat> EventSeat { get; }
	}
}
