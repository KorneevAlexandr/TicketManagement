using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.DataAccess.Interfaces.Repositories
{
	/// <summary>
	/// Represents additional manipulation possibilities Venue-entity.
	/// </summary>
	public interface IVenueRepository : IBaseRepository<Venue>
	{
		/// <summary>
		/// Getting Venue by his unique name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Venue.</returns>
		Task<Venue> GetByNameAsync(string name);

		/// <summary>
		/// Getting all venues from data set.
		/// In practice, there are not very many of them.
		/// </summary>
		/// <returns>Collection venues.</returns>
		Task<IQueryable<Venue>> GetAllAsync();
	}
}
