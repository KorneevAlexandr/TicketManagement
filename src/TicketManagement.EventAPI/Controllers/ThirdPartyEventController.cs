using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.ClientModels.Events;
using TicketManagement.EventAPI.Services.Interfaces;

namespace TicketManagement.EventAPI.Controllers
{
	/// <summary>
	/// Provides a method for receiving and processing a json file that stores information 
	/// about events provided from the ThirdPartyEvent Editor.
	/// Available to a user with a role ModeratorEvent.
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	[Authorize(Roles = "ModeratorEvent")]
	public class ThirdPartyEventController : Controller
	{
		private readonly IWebHostEnvironment _appEnvironment;
		private readonly IThirdPartyEventService _thirdPartyEventService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ThirdPartyEventController"/> class.
		/// Initializes services.
		/// </summary>
		/// <param name="appEnvironment">Environment for web root.</param>
		/// <param name="thirdPartyEventService">Service for manipulating third-party events.</param>
		public ThirdPartyEventController(IWebHostEnvironment appEnvironment, IThirdPartyEventService thirdPartyEventService)
		{
			_appEnvironment = appEnvironment;
			_thirdPartyEventService = thirdPartyEventService;
			_thirdPartyEventService.WebRootPath = _appEnvironment.WebRootPath;
		}

		[HttpPost("File")]
		public async Task<IActionResult> AddFile([FromForm] IFormFile uploadedFile)
		{
			string path = "/Files/" + uploadedFile.FileName;

			// save .json file in wwwroot
			using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
			{
				await uploadedFile.CopyToAsync(fileStream);
			}

			var proccessEvents = await _thirdPartyEventService.SaveThirdPartyEventsFromJsonFile(path);
			var message = _thirdPartyEventService.LastMessage;
			var dictionaryResponse = new Dictionary<string, List<ThirdPartyEvent>>
			{
				{ message, proccessEvents },
			};
			return Json(dictionaryResponse);
		}
	}
}
