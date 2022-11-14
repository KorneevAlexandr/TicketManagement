using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using ThirdPartyEventEditor.Filters;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Services;

namespace ThirdPartyEventEditor.Controllers
{
    /// <summary>
    /// Contoller for action, allowing export file.
    /// </summary>
    [Authorize (Roles = "ModeratorExport")]
    [LoggerTimeFilter]
    public class ExportController : Controller
    {
        private readonly IThirdPartyEventService _eventService;

        /// <summary>
        /// Inizialize controller.
        /// </summary>
        public ExportController(IThirdPartyEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Start action, load all events.
        /// </summary>
        /// <returns>View with all events.</returns>
        public async Task<ActionResult> Index()
        {
            var events = await _eventService.GetAllAsync();
            return View(events);
        }

        /// <summary>
        /// Details for selected event.
        /// </summary>
        /// <param name="id">Id selected event.</param>
        /// <returns>View with event.</returns>
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var item = await _eventService.GetAsync(id.Value);
            return View(item);
        }

        /// <summary>
        /// Build .json-file and export his in specified (Web.config) derictory.
        /// </summary>
        /// <param name="parameters">Flags events, which need to export.</param>
        /// <param name="fileName">New file with export.</param>
        /// <returns>View.</returns>
        [HttpPost]
        public async Task<ActionResult> ExportFile(List<int> parameters, string fileName)
        {
            if (parameters == null || !parameters.Any() || fileName == null || fileName.Trim() == string.Empty)
            {                
                return RedirectToAction("Index");
            }

            var events = await _eventService.GetAllAsync();
            var selectedEvents = events.Where(x => parameters.Contains(x.Id)).ToList();

            var exportEvents = selectedEvents.Select(x => new ExportThirdPartyEvent
            {
                Name = x.Name,
                Description = x.Description,
                LayoutName = x.LayoutName,
                VenueName = x.VenueName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PosterImage = x.PosterImage,
                NameImage = x.NameImage,
                Price = x.Price,
            });

            await WriteFileAsync(fileName, exportEvents);
            await _eventService.UpdateExportedAsync(selectedEvents);

            return RedirectToAction("Index");
        }

        private async Task WriteFileAsync(string fileName, IEnumerable<ExportThirdPartyEvent> collections)
        {
            string allowedFileType = WebConfigurationManager.AppSettings["AllowedFileTypes"];
            fileName = fileName.Contains(allowedFileType) ? fileName : fileName + allowedFileType;
            string path = WebConfigurationManager.AppSettings["ExportSourcePath"] + fileName;

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            };

            FileStream stream = null;
            lock (new object())
            {
                stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            }
            await JsonSerializer.SerializeAsync(stream, collections, options);
            stream.Close();
        }
    }
}