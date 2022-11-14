using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.BLInterfaces
{
	/// <summary>
	/// Represent provides the ability to manage third-party events from a file.
	/// </summary>
	public interface IThirdPartyEventService
	{
		/// <summary>
		/// Message about as a result of adding third-party events.
		/// </summary>
		string LastMessage { get; }

		/// <summary>
		/// Web root path for file. 
		/// Web root path not available for business logic and is passed explicitly.
		/// </summary>
		string WebRootPath { get; set; }

		/// <summary>
		/// Save valid third-party events in database, saving the import file and images.
		/// </summary>
		/// <param name="path">Path to json-file.</param>
		/// <returns>Events, marked as added or not added.</returns>
		Task<List<ThirdPartyEvent>> SaveThirdPartyEventsFromJsonFile(string path);
	}
}
