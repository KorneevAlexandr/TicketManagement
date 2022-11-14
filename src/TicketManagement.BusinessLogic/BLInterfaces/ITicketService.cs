using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.BLInterfaces
{
	/// <summary>
	/// Represents operations for proxy ticket-repository calls with validation logic.
	/// </summary>
	public interface ITicketService
	{
		/// <summary>
		/// Getting all tickets by user entity id.
		/// </summary>
		/// <param name="parentId">Parent (user) id.</param>
		/// <returns>Collections tickets.</returns>
		Task<IEnumerable<TicketDto>> GetAllAsync(int parentId);

		/// <summary>
		/// Add ticket in data set.
		/// </summary>
		/// <param name="entity">Ticket to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		Task<bool> AddAsync(TicketDto entity);

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
		Task<TicketDto> GetAsync(int id);

		/// <summary>
		/// Update ticket entity in data set.
		/// </summary>
		/// <param name="entity">Ticket to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		bool Update(TicketDto entity);
	}
}
