using System.Linq;
using TicketManagement.DataAccess.Domain;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces.Repositories
{
	/// <summary>
	/// Represent operations manipulate the tikcet entity in data set.
	/// </summary>
	public interface ITicketRepository
	{
		/// <summary>
		/// Getting all tickets by user entity id.
		/// </summary>
		/// <param name="parentId">Parent (user) id.</param>
		/// <returns>Collections tickets.</returns>
		Task<IQueryable<Ticket>> GetAllAsync(int parentId);

		/// <summary>
		/// Add ticket in data set.
		/// </summary>
		/// <param name="entity">Ticket to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		Task<bool> AddAsync(Ticket entity);

		/// <summary>
		/// Delete ticket from data set.
		/// </summary>
		/// <param name="id">Id ticket to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		bool Delete(int id);

		/// <summary>
		/// Gettin ticket to data set.
		/// </summary>
		/// <param name="id">Id ticket to getting.</param>
		/// <returns>Ticket.</returns>
		Task<Ticket> GetAsync(int id);

		/// <summary>
		/// Update ticket entity in data set.
		/// </summary>
		/// <param name="entity">Ticket to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		bool Update(Ticket entity);
	}
}
