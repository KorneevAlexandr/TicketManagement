using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.UnitOfWork;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.EventAPI.Models;
using TicketManagement.EventAPI.Services.Interfaces;
using TicketManagement.EventAPI.Services.ValidationProviders;

namespace TicketManagement.EventAPI.Services
{
	/// <summary>
	/// Represents calling event proxy repository with validation logic.
	/// </summary>
	public class EventService : IEventService
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="EventService"/> class.
		/// Create UnitOfWork-element.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public EventService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Get collection events with validation logic.
		/// </summary>
		/// <returns>Collections events.</returns>
		public async Task<IEnumerable<EventDto>> GetAllAsync(int parentId)
		{
			var events = await _unitOfWork.Event.GetAllAsync(parentId);

			return events.Select(evt => ConvertToEventDto(evt));
		}

		/// <summary>
		/// Getting collection events, satisfying the condition.
		/// </summary>
		/// <param name="start">Date-time start event.</param>
		/// <param name="end">Date-time end event.</param>
		/// <param name="partName">Part event name.</param>
		/// <returns>Collection events.</returns>
		public async Task<IEnumerable<EventDto>> GetAllAsync(DateTime start, DateTime end, string partName)
		{
			var events = await _unitOfWork.Event.GetAllAsync(start, end, partName);

			return events.Select(evt => ConvertToEventDto(evt));
		}

		/// <summary>
		/// Getting collection latest events.
		/// </summary>
		/// <param name="count">Count latest event.</param>
		/// <returns>Collection events.</returns>
		public async Task<IEnumerable<EventDto>> GetLatestAsync(int count)
		{
			var events = await _unitOfWork.Event.GetLatestAsync(count);
			return events.Select(evt => ConvertToEventDto(evt));
		}

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>Layout.</returns>
		public async Task<EventDto> GetAsync(int id)
		{
			EventValidationProvider.IsValidToDelete(id);

			var evt = await _unitOfWork.Event.GetAsync(id);
			return ConvertToEventDto(evt);
		}

		/// <summary>
		/// Add event with validation logic.
		/// Main validation - can't be created without any seats.
		/// </summary>
		/// <param name="entity">Event.</param>
		/// <returns>True - validation and adding successfull, false - successfull only validation.</returns>
		public async Task<bool> AddAsync(EventDto entity)
		{
			var userEvent = ConvertToEvent(entity);
			EventValidationProvider.IsValidToAdd(userEvent);

			await CheckLayoutIdExistAsync(userEvent);

			// check that we do not create event for the same venue in the same time.
			await CheckEventInSameTimeAddAsync(userEvent);

			// checking that event will have seats
			await CheckEventWillHaveSeatsAsync(userEvent);

			return await _unitOfWork.Event.AddAsync(userEvent);
		}

		/// <summary>
		/// Delete event by id with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier event.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			EventValidationProvider.IsValidToDelete(id);

			return await _unitOfWork.Event.DeleteAsync(id);
		}

		/// <summary>
		/// Update event with validation logic.
		/// Main validation - can't be created without any seats.
		/// </summary>
		/// <param name="entity">Event.</param>
		/// <returns>True - validation and updating successfull, false - successfull only validation.</returns>
		public async Task<bool> UpdateAsync(EventDto entity)
		{
			var userEvent = ConvertToEvent(entity);
			EventValidationProvider.IsValidToUpdate(userEvent);

			await CheckLayoutIdExistAsync(userEvent);

			// check that we do not create event for the same venue in the same time.
			await CheckEventIdSameTimeUpdateAsync(userEvent);

			// checking that event will have seats
			await CheckEventWillHaveSeatsAsync(userEvent);


			return await _unitOfWork.Event.UpdateAsync(userEvent);
		}

		private async Task CheckEventWillHaveSeatsAsync(Event userEvent)
		{
			var areas = await _unitOfWork.Area.GetAllAsync(userEvent.LayoutId);
			var areasId = areas.Select(x => x.Id);
			var seats = new List<Seat>();

			foreach (var areaId in areasId)
			{
				seats.AddRange(await _unitOfWork.Seat.GetAllAsync(areaId));
			}

			if (!seats.Any())
			{
				throw new InvalidOperationException("Cannot add new Event, when this event will not have seat.");
			}
		}

		private async Task CheckEventInSameTimeAddAsync(Event userEvent)
		{
			var events = await _unitOfWork.Event.GetAllAsync(userEvent.VenueId);
			var badEventsDT = events
						   .Where(x => (userEvent.DateTimeStart >= x.DateTimeStart && userEvent.DateTimeStart <= x.DateTimeEnd)
						   || (userEvent.DateTimeEnd >= x.DateTimeStart && userEvent.DateTimeEnd <= x.DateTimeEnd));

			if (badEventsDT.Any())
			{
				throw new InvalidOperationException("The specified time period is occupied by another event.");
			}
		}

		private async Task CheckEventIdSameTimeUpdateAsync(Event userEvent)
		{
			var events = await _unitOfWork.Event.GetAllAsync(userEvent.VenueId);

			var badEventsDT = events.Where(x => x.Id != userEvent.Id)
						   .Where(x => (userEvent.DateTimeStart >= x.DateTimeStart && userEvent.DateTimeStart <= x.DateTimeEnd)
						   || (userEvent.DateTimeEnd >= x.DateTimeStart && userEvent.DateTimeEnd <= x.DateTimeEnd));

			if (badEventsDT.Any())
			{
				throw new InvalidOperationException("The specified time period is occupied by another event.");
			}
		}


		private async Task CheckLayoutIdExistAsync(Event userEvent)
		{
			var result = await _unitOfWork.Layout.GetAsync(userEvent.LayoutId) != null;
			if (!result)
			{
				throw new InvalidOperationException("Invalid LayoutId: such layout does not exist.");
			}
		}

		private Event ConvertToEvent(EventDto eventDto)
		{
			return new Event
			{
				Id = eventDto.Id,
				Description = eventDto.Description,
				DateTimeStart = eventDto.DateTimeStart,
				DateTimeEnd = eventDto.DateTimeEnd,
				Name = eventDto.Name,
				LayoutId = eventDto.LayoutId,
				Price = eventDto.Price,
				State = (SeatState)eventDto.State,
				VenueId = eventDto.VenueId,
				URL = eventDto.URL,
			};
		}

		private EventDto ConvertToEventDto(Event simpleEvent)
		{
			return new EventDto
			{
				Id = simpleEvent.Id,
				Description = simpleEvent.Description,
				DateTimeStart = simpleEvent.DateTimeStart,
				DateTimeEnd = simpleEvent.DateTimeEnd,
				Name = simpleEvent.Name,
				LayoutId = simpleEvent.LayoutId,
				Price = simpleEvent.Price,
				State = (SeatStateDto)simpleEvent.State,
				VenueId = simpleEvent.VenueId,
				URL = simpleEvent.URL,
			};
		}
	}
}
