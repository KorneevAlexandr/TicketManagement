using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces.Repositories
{
	/// <summary>
	/// Represents operations manipulate the elements of a data set.
	/// Used to manipulate datasets regardless of the data source.
	/// </summary>
	/// <typeparam name="T">Entity, implement IHasBasicId.</typeparam>
	public interface IBaseRepository<T> 
		where T : class
	{
		/// <summary>
		/// Getting all elements by parent entity id.
		/// </summary>
		/// <param name="parentId">Parent id.</param>
		/// <returns>Collections elements.</returns>
		Task<IQueryable<T>> GetAllAsync(int parentId);

		/// <summary>
		/// Get one element by id.
		/// </summary>
		/// <param name="id">Id element.</param>
		/// <returns>Element.</returns>
		Task<T> GetAsync(int id);

		/// <summary>
		/// Add element in data set.
		/// </summary>
		/// <param name="entity">Element to add.</param>
		/// <returns>True - if successfull, else - false.</returns>
		Task<bool> AddAsync(T entity);

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
