using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.UserAPI.DataContext;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Repositories.Interfaces;

namespace TicketManagement.UserAPI.Repositories
{
	/// <summary>
	/// Provides methods to access a collection of tickets stored in a database.
	/// </summary>
	public class TicketRepository : ITicketRepository
	{
		private readonly UserContext _context;

		public TicketRepository(UserContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Getting all tickets specified user.
		/// </summary>
		/// <param name="userId">User id.</param>
		/// <returns>Collection of tickets.</returns>
		public async Task<IQueryable<Ticket>> GetAllAsync(int userId)
		{
			return await Task.Run(() => _context.Tickets.Where(x => x.UserId == userId));
		}
	}
}
