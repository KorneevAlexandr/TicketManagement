using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.EventAPI.Models;

namespace TicketManagement.EventAPI.Services.Interfaces
{
	/// <summary>
	/// Provides methods for getting various shapes venues and layouts.
	/// </summary>
	public interface IVenueLayoutGettingService
	{
		/// <summary>
		/// Getting all venues from data set.
		/// In practice, there are not very many of them.
		/// </summary>
		/// <returns>Collection venues.</returns>
		Task<IEnumerable<VenueDto>> GetAllVenuesAsync();

		/// <summary>
		/// Get collection elements with validation logic.
		/// </summary>
		/// <returns>Collections elements.</returns>
		Task<IEnumerable<LayoutDto>> GetAllLayoutsAsync(int venueId);

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>Element.</returns>
		Task<VenueDto> GetVenueAsync(int id);

		/// <summary>
		/// Get venue by his name.
		/// </summary>
		/// <param name="name">Venue name.</param>
		/// <returns>Venue.</returns>
		Task<VenueDto> GetVenueAsync(string name);
	}
}
