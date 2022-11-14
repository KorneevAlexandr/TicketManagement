using System.Threading.Tasks;
using System.Collections.Generic;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.BLInterfaces
{
	/// <summary>
	/// Represents operations for proxy venue-repository calls with validation logic.
	/// </summary>
	public interface IVenueService : IServiceBase<VenueDto>
	{
		/// <summary>
		/// Get venue with validation logic.
		/// </summary>
		/// <returns>Venue.</returns>
		Task<VenueDto> GetByNameAsync(string name);

		/// <summary>
		/// Getting all venues from data set.
		/// In practice, there are not very many of them.
		/// </summary>
		/// <returns>Collection venues.</returns>
		Task<IEnumerable<VenueDto>> GetAllAsync();
	}
}
