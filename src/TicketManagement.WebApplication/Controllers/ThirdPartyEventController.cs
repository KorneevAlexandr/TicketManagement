using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.WebApplication.Clients;

namespace TicketManagement.WebApplication.Controllers
{
    /// <summary>
    /// Provides operation for fetching the import file, adding and displaying third-party events.
    /// </summary>
    [Authorize(Roles = "ModeratorEvent")]
    public class ThirdPartyEventController : Controller
    {
        private readonly IEventRestClient _eventClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="ThirdPartyEventController"/> class.
        /// Initializes services.
		/// </summary>
		/// <param name="eventClient">Client for eventAPI.</param>
        public ThirdPartyEventController(IEventRestClient eventClient)
        {
            _eventClient = eventClient;
        }

        /// <summary>
        /// Start page.
        /// </summary>
        /// <returns>Start view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load selected import file in system.
        /// Saving file and processing.
        /// </summary>
        /// <param name="uploadedFile">Selected file.</param>
        /// <returns>View with third-party events.</returns>
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var proccessEvents = await _eventClient.AddFile(uploadedFile, HttpContext.Request.Cookies["Token"]);
                ViewData["Message"] = proccessEvents.FirstOrDefault().Key;
                return View(proccessEvents.FirstOrDefault().Value);
            }

            return RedirectToAction("Index");
        }
    }
}
