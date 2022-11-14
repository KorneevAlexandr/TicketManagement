using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories
{
	/// <summary>
	/// Represent methods for management entity ThirdPartyEvent.
	/// How the source is used .json file.
	/// </summary>
	public class ThirdPartyEventRepository : IThirdPartyEventRepository
	{
		private const int STEP_ID = 1;
		private const int START_ID = 1;

		private readonly string _path;
		private object _lockReadObject = new object();
		private object _lockWriteObject = new object();

		/// <summary>
		/// Inizialize path to file.
		/// </summary>
		/// <param name="path">Path to file.</param>
		public ThirdPartyEventRepository(string path)
		{
			_path = path;
		}

		/// <summary>
		/// Getting all ThirdPartyEvents.
		/// </summary>
		/// <returns>Collection ThirdPartyEvents.</returns>
		public async Task<IEnumerable<ThirdPartyEvent>> GetAllAsync()
		{
			var stream = GetReadStream();
			
			var events = await JsonSerializer.DeserializeAsync<IEnumerable<ThirdPartyEvent>>(stream);
			stream.Close();
			return events;
		}

		/// <summary>
		/// Get one ThirdPartyEvent by id.
		/// </summary>
		/// <param name="id">Id ThirdPartyEvent.</param>
		/// <returns>ThirdPartyEvent.</returns>
		public async Task<ThirdPartyEvent> GetAsync(int id)
		{
			var events = await GetAllAsync();

			return events.ToList().Find(x => x.Id == id);
		}

		/// <summary>
		/// Added ThirdPartyEvent.
		/// </summary>
		/// <param name="partyEvent">ThirdPartyEvent for added.</param>
		/// <returns>Id new object.</returns>
		public async Task<int> AddAsync(ThirdPartyEvent partyEvent)
		{
			var events = await GetAllAsync();
			var listEvents = events.ToList();

			partyEvent.Id = events.Any() ? events.Last().Id + STEP_ID : START_ID;
			listEvents.Add(partyEvent);

			await WriteToFileAsync(listEvents);

			return partyEvent.Id;
		}

		/// <summary>
		/// Delete ThirdPartyEvent by id.
		/// </summary>
		/// <param name="id">Id object.</param>
		/// <returns>Task.</returns>
		public async Task DeleteAsync(int id)
		{
			var events = await GetAllAsync();
			var listEvents = events.ToList();

			List<ThirdPartyEvent> newListEvents = new List<ThirdPartyEvent>();
			foreach (var item in listEvents)
			{
				if (item.Id != id)
				{
					newListEvents.Add(item);
				}
			}

			await WriteToFileAsync(newListEvents);
		}

		/// <summary>
		/// Update specified ThirdPartyEvent.
		/// </summary>
		/// <param name="partyEvent">ThirdPartyEvent for updated.</param>
		/// <returns>Task.</returns>
		public async Task UpdateAsync(ThirdPartyEvent partyEvent)
		{
			var events = await GetAllAsync();
			var listEvents = events.ToList();

			int index = listEvents.FindIndex(x => x.Id == partyEvent.Id);
			listEvents[index] = partyEvent;

			await WriteToFileAsync(listEvents);
		}

		private async Task WriteToFileAsync(IEnumerable<ThirdPartyEvent> partyEvents)
		{
			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
				WriteIndented = true,
			};

			var stream = GetWriteStream();
			await JsonSerializer.SerializeAsync(stream, partyEvents, options);
			stream.Close();
		}

		private FileStream GetReadStream()
		{
			FileStream stream = null;
			lock (_lockReadObject)
			{
				stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
			}
			return stream;
		}

		private FileStream GetWriteStream()
		{
			FileStream stream = null;
			lock (_lockWriteObject)
			{
				stream = new FileStream(_path, FileMode.Create, FileAccess.Write);
			}
			return stream;
		}
	}
}