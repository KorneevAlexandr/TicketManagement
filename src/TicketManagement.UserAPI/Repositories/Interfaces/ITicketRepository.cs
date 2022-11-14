using System.Linq;
using System.Threading.Tasks;
using TicketManagement.UserAPI.Models;

namespace TicketManagement.UserAPI.Repositories.Interfaces
{
	public interface ITicketRepository
	{
		Task<IQueryable<Ticket>> GetAllAsync(int userId);
	}
}
