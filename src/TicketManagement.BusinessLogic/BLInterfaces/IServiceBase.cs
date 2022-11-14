using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TicketManagement.BusinessLogic.BLInterfaces
{
	/// <summary>
	/// Represents operations for proxy repository calls with validation logic.
	/// </summary>
	/// <typeparam name="T">Any type of domain scope.</typeparam>
	public interface IServiceBase<T>
	    where T : class
	{
		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>Element.</returns>
		Task<T> GetAsync(int id);

		/// <summary>
		/// Get collection elements with validation logic.
		/// </summary>
		/// <returns>Collections elements.</returns>
		Task<IEnumerable<T>> GetAllAsync(int parentId);

		/// <summary>
		/// Add element with validation logic.
		/// </summary>
		/// <param name="entity">Element of the specified type.</param>
		/// <returns>True - validation and adding successfull, false - successfull only validation.</returns>
		Task<bool> AddAsync(T entity);

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
		Task<bool> UpdateAsync(T entity);
	}
}
