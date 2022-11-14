using TicketManagement.DataAccess.Domain;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace TicketManagement.DataAccess.Interfaces.Repositories
{
	/// <summary>
	/// Represents operations for proxy event-repository calls with validation logic.
	/// </summary>
	public interface IEventRepository : IBaseRepository<Event>
	{
		/// <summary>
		/// Getting collection events, satisfying the condition.
		/// </summary>
		/// <param name="start">Date-time start event.</param>
		/// <param name="end">Date-time end event.</param>
		/// <param name="partName">Part event name.</param>
		/// <returns>Collection events.</returns>
		Task<IQueryable<Event>> GetAllAsync(DateTime start, DateTime end, string partName);

		/// <summary>
		/// Getting collection latest events.
		/// </summary>
		/// <param name="count">Count latest event.</param>
		/// <returns>Collection events.</returns>
		Task<IQueryable<Event>> GetLatestAsync(int count);
	}
}
