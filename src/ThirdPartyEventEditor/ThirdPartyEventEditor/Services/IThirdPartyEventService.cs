using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Services
{
	/// <summary>
	/// Represent methods for management entity ThirdPartyEvent.
	/// Used in business-logic.
	/// </summary>
	public interface IThirdPartyEventService
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
		/// Updated collection events, changes value exported.
		/// </summary>
		/// <param name="partyEvents">Collection events.</param>
		/// <returns>Task.</returns>
		Task UpdateExportedAsync(List<ThirdPartyEvent> partyEvents);

		/// <summary>
		/// Delete ThirdPartyEvent by id.
		/// </summary>
		/// <param name="id">Id object.</param>
		/// <returns>Task.</returns>
		Task DeleteAsync(int id);
	}
}
