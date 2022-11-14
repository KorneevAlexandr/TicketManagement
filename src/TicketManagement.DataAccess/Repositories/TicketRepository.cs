using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entity;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.Repositories;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Repositories
{
	/// <summary>
	/// Represent implement operations manipulate the tikcet entity in data set.
	/// </summary>
	internal class TicketRepository : ITicketRepository
	{
		private readonly ApplicationDbContext _context;

		public TicketRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Getting all tickets by user entity id.
		/// </summary>
		/// <param name="parentId">Parent (user) id.</param>
		/// <returns>Collections tickets.</returns>
		public async Task<IQueryable<Ticket>> GetAllAsync(int parentId)
		{
			var tickets = await _context.Tickets.Where(ticket => ticket.UserId == parentId).ToListAsync();

			return tickets.AsQueryable();
		}

		/// <summary>
		/// Add ticket in data set.
		/// </summary>
		/// <param name="entity">Ticket to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> AddAsync(Ticket entity)
		{
			if (entity == null)
			{
				return false;
			}

			await _context.Tickets.AddAsync(entity);
			SaveChangeAsync();
			return true;
		}

		/// <summary>
		/// Delete ticket from data set.
		/// </summary>
		/// <param name="id">Id ticket to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Delete(int id)
		{
			if (id <= 0)
			{
				return false;
			}

			_context.Tickets.Remove(new Ticket { Id = id });
			SaveChangeAsync();
			return true;
		}

		/// <summary>
		/// Gettin ticket to data set.
		/// </summary>
		/// <param name="id">Id ticket to getting.</param>
		/// <returns>Ticket.</returns>
		public async Task<Ticket> GetAsync(int id)
		{
			return await _context.Tickets.FindAsync(id);
		}

		/// <summary>
		/// Update ticket entity in data set.
		/// </summary>
		/// <param name="entity">Ticket to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Update(Ticket entity)
		{
			if (entity == null)
			{
				return false;
			}

			_context.Tickets.Update(entity);

			SaveChangeAsync();
			return true;
		}

		private void SaveChangeAsync()
		{
			_context.SaveChangesAsync();
		}
	}
}
