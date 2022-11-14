using RestEase;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Events;

namespace TicketManagement.ReactUI.ApiClient
{
	/// <summary>
	/// A client interface that provides methods for calling methods of the event-handling service.
	/// </summary>
	public interface IEventRestClient
	{
		[Get("eventManagement/LatestEvents")]
		Task<List<EventModel>> GetLatestEvents(int count, [Header("Token")] string token);

		[Get("eventManagement/EventsByNameAndDate")]
		Task<List<EventModel>> GetEventsByNameAndDate(string partEventName,
			[Header("StartDate")] string startDate, [Header("EndDate")] string endDate, [Header("Token")] string token);

		[Get("eventManagement/Events")]
		Task<EventCollectionModel> GetEvents(int venueId, [Header("Token")] string token);

		[Get("eventManagement/Event")]
		Task<EventFullModel> GetEvent(int eventId, [Header("Token")] string token);

		[Delete("eventManagement/Event")]
		Task DeleteEvent(int eventId, [Header("Token")] string token);

		[Post("eventManagement/Event")]
		Task CreateEvent([Body] EventTransportModel model, [Header("Token")] string token);

		[Put("eventManagement/Event")]
		Task UpdateEvent([Body] EventTransportModel model, [Header("Token")] string token);

		[Get("eventAreaManagement/EventAreas")]
		Task<IEnumerable<EventAreaModel>> GetEventAreas(int eventId, [Header("Token")] string token);

		[Get("eventAreaManagement/EventArea")]
		Task<EventAreaModel> GetEventArea(int eventAreaId, [Header("Token")] string token);

		[Put("eventAreaManagement/EventArea")]
		Task UpdateEventArea([Body] EventAreaModel model, [Header("Token")] string token);

		[Delete("eventAreaManagement/EventArea")]
		Task DeleteEventArea(int eventAreaId, [Header("Token")] string token);

		[Get("eventSeatManagement/EventSeats")]
		Task<EventSeatModel> GetEventSeats(int eventAreaId, [Header("Token")] string token);

		[Put("eventSeatManagement/EventSeatState")]
		Task UpdateEventSeatState(int eventSeatId, [Header("Token")] string token);

		[Post("thirdPartyEvent/File")]
		Task<string> AddFile([Body] HttpContent content);
	}
}
