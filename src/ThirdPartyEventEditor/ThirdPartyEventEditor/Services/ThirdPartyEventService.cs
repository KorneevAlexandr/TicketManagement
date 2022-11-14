using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Exceptions;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Repositories;

namespace ThirdPartyEventEditor.Services
{
	/// <summary>
	/// Represent methods for validate ThirdPartyEvent.
	/// Uses additional validation methods.
	/// </summary>
	public class ThirdPartyEventService : IThirdPartyEventService
	{
		private readonly IThirdPartyEventRepository _repository;

		/// <summary>
		/// Inizialize service with specified repository.
		/// </summary>
		/// <param name="repository">Repository.</param>
		public ThirdPartyEventService(IThirdPartyEventRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Inizialize service with specified path for repository.
		/// </summary>
		/// <param name="path">Path to file.</param>
		public ThirdPartyEventService(string path)
		{
			_repository = new ThirdPartyEventRepository(path);
		}

		/// <summary>
		/// Getting all ThirdPartyEvents.
		/// </summary>
		/// <returns>Collection ThirdPartyEvents.</returns>
		public async Task<IEnumerable<ThirdPartyEvent>> GetAllAsync()
		{
			return await _repository.GetAllAsync();
		}

		/// <summary>
		/// Get one ThirdPartyEvent by id.
		/// </summary>
		/// <param name="id">Id ThirdPartyEvent.</param>
		/// <returns>ThirdPartyEvent.</returns>
		public async Task<ThirdPartyEvent> GetAsync(int id)
		{
			IsValidId(id);
			return await _repository.GetAsync(id);
		}

		/// <summary>
		/// Added ThirdPartyEvent.
		/// </summary>
		/// <param name="partyEvent">ThirdPartyEvent for added.</param>
		/// <returns>Id new object.</returns>
		public async Task<int> AddAsync(ThirdPartyEvent partyEvent)
		{
			IsValidDateTime(partyEvent.StartDate, partyEvent.EndDate);
			var oldEvents = await GetAllAsync();
			IsUniqueName(oldEvents, partyEvent);
			return await _repository.AddAsync(partyEvent);
		}

		/// <summary>
		/// Delete ThirdPartyEvent by id.
		/// </summary>
		/// <param name="id">Id object.</param>
		/// <returns>Task.</returns>
		public async Task DeleteAsync(int id)
		{
			IsValidId(id);
			await _repository.DeleteAsync(id);
		}

		/// <summary>
		/// Update specified ThirdPartyEvent.
		/// </summary>
		/// <param name="partyEvent">ThirdPartyEvent for updated.</param>
		/// <returns>Task.</returns>
		public async Task UpdateAsync(ThirdPartyEvent partyEvent)
		{
			IsValidId(partyEvent.Id);
			IsValidDateTime(partyEvent.StartDate, partyEvent.EndDate);
			var oldEvent = await GetAsync(partyEvent.Id);
			if (!oldEvent.Name.Equals(partyEvent.Name))
			{
				var allEvents = await _repository.GetAllAsync();
				IsUniqueName(allEvents, partyEvent);
			}

			await _repository.UpdateAsync(partyEvent); 
		}

		/// <summary>
		/// Updated collection events, changes value exported.
		/// </summary>
		/// <param name="partyEvents">Collection events.</param>
		/// <returns>Task.</returns>
		public async Task UpdateExportedAsync(List<ThirdPartyEvent> partyEvents)
		{
			for (int i = 0; i < partyEvents.Count; i++)
			{
				var updatedEvent = partyEvents[i];
				if (!updatedEvent.Exported)
				{
					updatedEvent.Exported = true;
					await _repository.UpdateAsync(updatedEvent);
				}
			}
		}

		private void IsValidDateTime(DateTime dtStart, DateTime dtEnd)
		{
			if (dtStart >= dtEnd)
			{
				throw new EventManagementException("Start date cannot be later than end date.",
					"Specify start date later than end date.", "Entered dates");
			}
			if (dtStart < DateTime.Now && dtEnd < DateTime.Now)
			{
				throw new EventManagementException("Event can not created in past.",
					"Specify the start date and end date of the event for future time.", "Entered dates");
			}
		}

		private void IsUniqueName(IEnumerable<ThirdPartyEvent> allEvents, ThirdPartyEvent partyEvent)
		{
			if (allEvents.ToList().FirstOrDefault(x => x.Name.Equals(partyEvent.Name)) != null)
			{
				throw new EventManagementException($"Name '{partyEvent.Name}' is not unique.",
					"Enter other event name.", "Event name");
			}
		}

		private void IsValidId(int id)
		{
			if (id <= 0)
			{
				throw new EventManagementException("Id can not be less or equal to 0.", 
					"Selected other event.", "ID event");
			}
		}

	}
}