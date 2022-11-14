using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories
{
	/// <summary>
	/// Represent methods for management entity ThirdPartyEvent.
	/// </summary>
	public interface IThirdPartyEventRepository
	{
		/// <summary>
		/// Getting all ThirdPartyEvents.
		/// </summary>
		/// <returns>Collection ThirdPartyEvents.</returns>
		Task<IEnumerable<ThirdPartyEvent>> GetAllAsync();

		/// <summary>
		/// Get one ThirdPartyEvent by id.
		/// </summary>
		/// <param name="id">Id ThirdPartyEvent.</param>
		/// <returns>ThirdPartyEvent.</returns>
		Task<ThirdPartyEvent> GetAsync(int id);

		/// <summary>
		/// Added ThirdPartyEvent.
		/// </summary>
		/// <param name="partyEvent">ThirdPartyEvent for added.</param>
		/// <returns>Id new object.</returns>
		Task<int> AddAsync(ThirdPartyEvent partyEvent);

		/// <summary>
		/// Update specified ThirdPartyEvent.
		/// </summary>
		/// <param name="partyEvent">ThirdPartyEvent for updated.</param>
		/// <returns>Task.</returns>
		Task UpdateAsync(ThirdPartyEvent partyEvent);

		/// <summary>
		/// Delete ThirdPartyEvent by id.
		/// </summary>
		/// <param name="id">Id object.</param>
		/// <returns>Task.</returns>
		Task DeleteAsync(int id);
	}
}