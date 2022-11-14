using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.EventAPI.Models;

namespace TicketManagement.EventAPI.Services.Interfaces
{
	/// <summary>
	/// Represent operations manipulate the events entity in data set.
	/// </summary>
	public interface IEventService
	{
		/// <summary>
		/// Get collection elements with validation logic.
		/// </summary>
		/// <returns>Collections elements.</returns>
		Task<IEnumerable<EventDto>> GetAllAsync(int parentId);

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

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>Element.</returns>
		Task<EventDto> GetAsync(int id);

		/// <summary>
		/// Add element with validation logic.
		/// </summary>
		/// <param name="entity">Element of the specified type.</param>
		/// <returns>True - validation and adding successfull, false - successfull only validation.</returns>
		Task<bool> AddAsync(EventDto entity);

		/// <summary>
		/// Delete element by unique identifier with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		Task<bool> DeleteAsync(int id);

		/// <summary>
		/// Update element with validation logic.
		/// </summary>
		/// <param name="entity">Element of the specified type.</param>
		/// <returns>True - validation and update successfull, false - successfull only validation.</returns>
		Task<bool> UpdateAsync(EventDto entity);
	}
}
