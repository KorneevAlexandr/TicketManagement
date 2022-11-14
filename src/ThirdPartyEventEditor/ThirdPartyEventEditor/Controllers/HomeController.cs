using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using ThirdPartyEventEditor.ConfigurationServices;
using ThirdPartyEventEditor.Filters;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Services;

namespace ThirdPartyEventEditor.Controllers
{
    /// <summary>
    /// Controller represent action for event-management.
    /// </summary>
    [Authorize (Roles = "ModeratorEvent")]
    [LoggerTimeFilter]
    public class HomeController : Controller
    {
        private readonly IThirdPartyEventService _eventService;
        private readonly string _venueName;

        /// <summary>
        /// Inizialize controller.
        /// </summary>
        public HomeController(IThirdPartyEventService eventService)
        {
            _venueName = WebConfigurationManager.AppSettings["VenueName"];
            _eventService = eventService;
        }

        /// <summary>
        /// Start action with all events.
        /// </summary>
        /// <returns>View with all events.</returns>
        public async Task<ActionResult> Index()
        {
            var events = await _eventService.GetAllAsync();
            return View(events.ToList());
        }

        /// <summary>
        /// Action create.
        /// </summary>
        /// <returns>View.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Action for create specified event.
        /// </summary>
        /// <param name="partyEvent">Event.</param>
        /// <returns>View.</returns>
        [HttpPost]
        public async Task<ActionResult> Create(ThirdPartyEvent partyEvent)
        {
            if (ModelState.IsValid)
            {
                partyEvent.VenueName = _venueName;
                partyEvent.PosterImage = await UploadSampleImage(partyEvent.NameImage);
                await _eventService.AddAsync(partyEvent);
                return RedirectToAction("Index");
            }
            return View(partyEvent);
        }

        /// <summary>
        /// Action details.
        /// </summary>
        /// <param name="id">Id selected event.</param>
        /// <returns>View with selected event.</returns>
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var item = await _eventService.GetAsync(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        /// <summary>
        /// Action delete.
        /// </summary>
        /// <param name="id">Selected id for delete event.</param>
        /// <returns>View.</returns>
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            await _eventService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action update.
        /// </summary>
        /// <param name="id">Id selected event for update.</param>
        /// <returns>View with selected event.</returns>
        [HttpGet]
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var partyEvent = await _eventService.GetAsync(id.Value);
            return View(partyEvent);
        }

        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="partyEvent">Updated event.</param>
        /// <returns>View.</returns>
        [HttpPost]
        public async Task<ActionResult> Update(ThirdPartyEvent partyEvent)
        {
            if (ModelState.IsValid)
            {
                partyEvent.VenueName = _venueName;
                partyEvent.PosterImage = await UploadSampleImage(partyEvent.NameImage);
                await _eventService.UpdateAsync(partyEvent);
                return RedirectToAction("Index");
            }
            return View(partyEvent);
        }

        /// <summary>
        /// Action for redirect on general error-page.
        /// </summary>
        /// <param name="title">Title exception.</param>
        /// <param name="message">Message exception.</param>
        /// <returns>View with data about exception.</returns>
        [HttpGet]
        public ActionResult Error(string title, string message)
        {
            ViewData["TypeException"] = title;
            ViewData["Name"] = message;
            return View();
        }

        private async Task<string> UploadSampleImage(string imageName)
        {
            var path = Path.Combine(Server.MapPath(WebConfigurationManager.AppSettings["ImagesSourcePath"]), imageName);
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}