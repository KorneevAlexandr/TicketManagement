using RestEase;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Events;
using TicketManagement.ClientModels.Tickets;

namespace TicketManagement.WebApplication.Clients
{
	/// <summary>
	/// A client interface that provides methods for calling methods of the purchase service.
	/// </summary>
	public interface IPurchaseRestClient
	{
		[Get("purchase/EventTickets")]
		Task<EventFullModel> GetEventTickets(int eventId, [Header("Token")] string token);

		[Get("purchase/EventSeats")]
		Task<EventSeatModel> GetEventSeats(int eventAreaId, [Header("Token")] string token);

		[Get("purchase/Ticket")]
		Task<TicketForBuyModel> GetReadyTicket(int eventSeatId, [Header("Token")] string token);

		[Post("purchase/Ticket")]
		Task DealTicket(int eventSeatId, [Header("Token")] string token);
	}
}
