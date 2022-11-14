using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces.Repositories
{
	/// <summary>
	/// Repository for manipulate EventArea and EventSeat.
	/// </summary>
	/// <typeparam name="T">EventArea or EventSeat.</typeparam>
	public interface IEventPlaceRepository<T>
		where T : class
	{
		/// <summary>
		/// Getting a collection places from a data set, owned by parent entity.
		/// </summary>
		/// <param name="id">Any parent entity.</param>
		/// <returns>Collection seats or areas, owned by event.</returns>
		Task<IQueryable<T>> GetPlacesAsync(int id);

		/// <summary>
		/// Getting a collection places from a data set, owned be event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection seats or areas, owned by event.</returns>
		Task<IQueryable<T>> GetPlacesByEventIdAsync(int id);

		/// <summary>
		/// Getting a entity by id.
		/// </summary>
		/// <param name="id">Id entity.</param>
		/// <returns>Entity.</returns>
		Task<T> GetAsync(int id);

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
