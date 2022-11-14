using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TicketManagement.WebApplication.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using TicketManagement.WebApplication.Clients;

namespace TicketManagement.WebApplication.Controllers
{
	/// <summary>
	/// Provides operation for home page.
	/// </summary>
	public class HomeController : Controller
	{
		private const int NUMEROUS_START_EVENT = 5;

		private readonly IEventRestClient _eventClient;

		public HomeController(IEventRestClient eventClient)
		{
			_eventClient = eventClient;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var eventsModel = await _eventClient.GetLatestEvents(NUMEROUS_START_EVENT, HttpContext.Request.Cookies["Token"]);
			return View(eventsModel);
		}

		[HttpGet]
		public IActionResult FAQ()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Poster(DateTime? start, DateTime? end, string partName)
		{
			start ??= new DateTime(1800, 1, 1);
			end ??= DateTime.MaxValue;
			partName ??= string.Empty;

			var events = await _eventClient.GetEventsByNameAndDate(
				partName, start.Value.ToShortDateString(), end.Value.ToShortDateString(), HttpContext.Request.Cookies["Token"]);
			return View(events);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

			return LocalRedirect(returnUrl);
		}
	}
}
