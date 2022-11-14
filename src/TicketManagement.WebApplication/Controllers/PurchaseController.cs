using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TicketManagement.WebApplication.Clients;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides operations for choosing a seat, buying tickets
	/// Not available for Admin role.
	/// </summary>
	[Authorize(Roles = "User, ModeratorVenue, ModeratorEvent")]
	public class PurchaseController : Controller
	{
		private readonly IPurchaseRestClient _purchaseClient;

		public PurchaseController(IPurchaseRestClient purchaseClient)
		{
			_purchaseClient = purchaseClient;
		}

		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Show event and areas for buy ticket.
		/// </summary>
		/// <param name="id">Event id.</param>
		/// <returns>View.</returns>
		[AllowAnonymous]
		public async Task<IActionResult> ShowEventBuy(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _purchaseClient.GetEventTickets(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		/// <summary>
		/// Show map seats.
		/// </summary>
		/// <param name="id">EventArea id.</param>
		/// <returns>View.</returns>
		public async Task<IActionResult> Seats(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _purchaseClient.GetEventSeats(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(model);
		}

		/// <summary>
		/// Show all info about future ticket.
		/// </summary>
		/// <param name="id">EventSeat id.</param>
		/// <returns>View.</returns>
		public async Task<IActionResult> BeforeBuyTicket(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var ticketModel = await _purchaseClient.GetReadyTicket(id.Value, HttpContext.Request.Cookies["Token"]);
			return View(ticketModel);
		}

		/// <summary>
		/// Buy ticket.
		/// </summary>
		/// <param name="id">Eventseat id.</param>
		/// <returns>View.</returns>
		[HttpPost]
		public async Task<IActionResult> DealTicket(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			await _purchaseClient.DealTicket(id.Value, HttpContext.Request.Cookies["Token"]);
			return await Task.Run(() => RedirectToAction("Index", "User"));
		}
	}
}
