using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketManagement.EventAPI.Services.Interfaces
{
	/// <summary>
	/// Represents operations for proxy eventSeat and eventArea repository calls with validation logic.
	/// </summary>
	public interface IEventPlaceService<T>
		where T : class
	{
		Task<T> GetAsync(int id);

		/// <summary>
		/// Getting a collection places from a data set, owned by event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection seats, owned by event.</returns>
		Task<IEnumerable<T>> GetPlacesAsync(int id);

		/// <summary>
		/// Getting a collection places from a data set, owned be event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection seats or areas, owned by event.</returns>
		Task<IEnumerable<T>> GetPlacesByEventIdAsync(int id);

		/// <summary>
		/// Delete a element by unique identifier from a data set.
		/// </summary>
		/// <param name="id">Unique identifier.</param>
		/// <returns>True - if successfull, else - false.</returns>
		Task<bool> DeleteAsync(int id);

		/// <summary>
		/// Update a element from a data set.
		/// </summary>
		/// <param name="entity">Element to update.</param>
		/// <returns>True - if successfull, else - false.</returns>
		Task<bool> UpdateAsync(T entity);
	}
}
