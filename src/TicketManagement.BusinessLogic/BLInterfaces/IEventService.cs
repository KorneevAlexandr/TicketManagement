using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.BLInterfaces
{
	/// <summary>
	/// Represent operations manipulate the events entity in data set.
	/// </summary>
	public interface IEventService : IServiceBase<EventDto>
	{
		/// <summary>
		/// Getting collection events, satisfying the condition.
		/// </summary>
		/// <param name="start">Date-time start event.</param>
		/// <param name="end">Date-time end event.</param>
		/// <param name="partName">Part event name.</param>
		/// <returns>Collection events.</returns>
		Task<IEnumerable<EventDto>> GetAllAsync(DateTime start, DateTime end, string partName);

		/// <summary>
		/// Getting collection latest events.
		/// </summary>
		/// <param name="count">Count latest event.</param>
		/// <returns>Collection events.</returns>
		Task<IEnumerable<EventDto>> GetLatestAsync(int count);
	}
}
