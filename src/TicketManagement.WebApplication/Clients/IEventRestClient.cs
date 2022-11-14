using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestEase;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Events;

namespace TicketManagement.WebApplication.Clients
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

	/// <summary>
	/// Extensions for EventRestClient. 
	/// Provides extension methods for easily accessing and invoking client methods.
	/// </summary>
	public static class EventRestClientExtensions
	{
		/// <summary>
		/// Adding the events stored in the passed json file(IFormFile) to data source.
		/// </summary>
		/// <param name="eventClient">Client Api.</param>
		/// <param name="file">Json file with third-party events.</param>
		/// <param name="token">User token.</param>
		/// <returns>A dictionary, where string is the message about loading events into the data source, 
		/// and the collection of events is the events handled.</returns>
		public static async Task<Dictionary<string, List<ThirdPartyEvent>>> AddFile(this IEventRestClient eventClient, IFormFile file, string token)
		{
			byte[] data;
			using (var br = new BinaryReader(file.OpenReadStream()))
			{
				data = br.ReadBytes((int)file.OpenReadStream().Length);
			}
			var bytes = new ByteArrayContent(data);
			var form = new MultipartFormDataContent
			{
				{ bytes, "uploadedFile", file.FileName },
			};
			form.Headers.Add("Token", token);

			var response = await eventClient.AddFile(form);
			return JsonConvert.DeserializeObject<Dictionary<string, List<ThirdPartyEvent>>>(response);
		}
	}
}
