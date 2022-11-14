using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Managing third party events, added their in database.
    /// </summary>
	internal class ThirdPartyEventService : IThirdPartyEventService
	{
        private const string IMAGE_EXTENSION = ".png";
        private const string IMAGE_FOLDER = @"\Images\";
        private const string SUCCESS_MESSAGE = "File fully is correct. \nAll event added in that system";
        private const string VENUE_FAIL_MESSAGE = "The venue specified in the file is not supported. \nAdd this venue with layouts or change the import file";
        private const string LAYOUT_FAIL_MESSAGE = "Not all of the specified layouts exist. \nAdd these layouts to the system or change the import file";
        private const string TIME_FAIL_MESSAGE = "Time for some events is already taken";

        private readonly IVenueService _venueService;
        private readonly IEventService _eventService;
        private readonly IServiceBase<LayoutDto> _layoutService;
        private string _webRootPath;
        private string _lastMessage;

        public ThirdPartyEventService(IVenueService venueService, IEventService eventService,
            IServiceBase<LayoutDto> layoutService)
        {
            _venueService = venueService;
            _eventService = eventService;
            _layoutService = layoutService;
        }

        /// <summary>
        /// Web root path for file. 
        /// Web root path not available for business logic and is passed explicitly.
        /// </summary>
        public string WebRootPath
        {
            get => _webRootPath;
            set { _webRootPath = value; }
        }

        /// <summary>
        /// Message about as a result of adding third-party events.
        /// </summary>
        public string LastMessage
        {
            get => _lastMessage;
            private set { _lastMessage = value; }
        }

        /// <summary>
        /// Save valid third-party events in database, saving the import file and images.
        /// </summary>
        /// <param name="path">Path to json-file.</param>
        /// <returns>Events, marked as added or not added.</returns>
        public async Task<List<ThirdPartyEvent>> SaveThirdPartyEventsFromJsonFile(string path)
        {
            var stream = new FileStream(_webRootPath + path, FileMode.Open, FileAccess.Read);
            var thirdPartyEvents = await JsonSerializer.DeserializeAsync<IEnumerable<ThirdPartyEvent>>(stream);
            stream.Close();
            return await ImportEvents(thirdPartyEvents);
        }

		// import data to database
        private async Task<List<ThirdPartyEvent>> ImportEvents(IEnumerable<ThirdPartyEvent> thirdPartyEvents)
        {
            LastMessage = SUCCESS_MESSAGE;
            var eventList = thirdPartyEvents.ToList();
            foreach (var item in eventList)
            {
                var venue = await _venueService.GetByNameAsync(item.VenueName);
                if (venue != null)
                {
                    var layouts = await _layoutService.GetAllAsync(venue.Id);
                    if (layouts.Select(x => x.Name).Contains(item.LayoutName))
                    {
                        string base64 = item.PosterImage.Substring(item.PosterImage.IndexOf(',') + 1);
                        byte[] data = Convert.FromBase64String(base64);
                        var fileName = GetRandomImageName(item);
                        var file = _webRootPath + fileName;
                        File.WriteAllBytes(file, data);

                        var oldEvent = new EventDto
                        {
                            Name = item.Name,
                            LayoutId = layouts.First(x => x.Name.Equals(item.LayoutName)).Id,
                            DateTimeStart = item.StartDate,
                            DateTimeEnd = item.EndDate,
                            Description = item.Description,
                            Price = Convert.ToDecimal(item.Price),
                            VenueId = venue.Id,
                            State = SeatStateDto.Free,
                            URL = fileName,
                        };
                        try
                        {
                            await _eventService.AddAsync(oldEvent);
                            item.Imported = true;
                        }
                        catch
                        {
                            LastMessage = TIME_FAIL_MESSAGE;
                        }
                    }
                    else
                    {
                        LastMessage = LAYOUT_FAIL_MESSAGE;
                    }
                }
                else
                {
                    LastMessage = VENUE_FAIL_MESSAGE;
                }
            }

            return eventList;
        }

        private string GetRandomImageName(ThirdPartyEvent partyEvent)
        {
            if (partyEvent.NameImage != null)
            {
                return $"{IMAGE_FOLDER}{partyEvent.NameImage}";
            }

            var path = $"{_webRootPath}{IMAGE_FOLDER}";
            var rnd = new Random();
            string fileName = $"{partyEvent.Name}{rnd.Next(1, 1000)}{IMAGE_EXTENSION}";

            while (File.Exists($"{path}{fileName}"))
            {
                fileName = $"{partyEvent.Name}{rnd.Next(1, 1000)}{IMAGE_EXTENSION}";
            }
            return $"{IMAGE_FOLDER}{fileName}";
        }
    }
}
