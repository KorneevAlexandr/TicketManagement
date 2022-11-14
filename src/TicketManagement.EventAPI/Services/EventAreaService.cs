using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DataAccess.UnitOfWork;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.DataAccess.Domain;
using TicketManagement.EventAPI.Models;
using TicketManagement.EventAPI.Services.Interfaces;
using TicketManagement.EventAPI.Services.ValidationProviders;

namespace TicketManagement.EventAPI.Services
{
	/// <summary>
	/// Represents calling eventArea proxy repository with validation logic.
	/// </summary>
	public class EventAreaService : IEventPlaceService<EventAreaDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="EventAreaService"/> class.
		/// Create UnitOfWork-element.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public EventAreaService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>EventArea.</returns>
		public async Task<EventAreaDto> GetAsync(int id)
		{
			EventValidationProvider.IsValidEventToPlaces(id);

			var eventArea = await _unitOfWork.EventArea.GetAsync(id);
			return ConvertToEventAreaDto(eventArea);
		}

		/// <summary>
		/// Get collection event-areas with validation logic.
		/// </summary>
		/// <returns>Collections events.</returns>
		public async Task<IEnumerable<EventAreaDto>> GetPlacesAsync(int id)
		{
			EventValidationProvider.IsValidEventToPlaces(id);

			var eventAreas = await _unitOfWork.EventArea.GetPlacesAsync(id);

			return eventAreas.Select(eventArea => ConvertToEventAreaDto(eventArea)).AsEnumerable();
		}

		/// <summary>
		/// Getting a collection places from a data set, owned be event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection areas, owned by event.</returns>
		public async Task<IEnumerable<EventAreaDto>> GetPlacesByEventIdAsync(int id)
		{
			return await GetPlacesAsync(id);
		}

		/// <summary>
		/// Delete event-area by id with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier event.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			EventAreaValidationProvider.IsValidToDelete(id);
			var eventArea = await _unitOfWork.EventArea.GetAsync(id);

			var seats = await _unitOfWork.EventSeat.GetPlacesAsync(eventArea.Id);
			if (seats.Any())
			{
				foreach (var seat in seats)
				{
					await _unitOfWork.EventSeat.DeleteAsync(seat.Id);
				}
			}

			return await _unitOfWork.EventArea.DeleteAsync(eventArea.Id);
		}

		/// <summary>
		/// Update event-area with validation logic.
		/// Main validation - can't be created without any seats.
		/// </summary>
		/// <param name="entity">EventArea.</param>
		/// <returns>True - validation and updating successfull, false - successfull only validation.</returns>
		public async Task<bool> UpdateAsync(EventAreaDto entity)
		{
			var eventArea = ConvertToEventArea(entity);
			EventAreaValidationProvider.IsValidToUpdate(eventArea);

			var changeEventArea = await _unitOfWork.EventArea.GetAsync(eventArea.Id);
			ExceptionIfNotEqualId(eventArea, changeEventArea);

			return await _unitOfWork.EventArea.UpdateAsync(eventArea);
		}

		private void ExceptionIfNotEqualId(EventArea eventArea, EventArea changeEventArea)
		{
			if (eventArea.EventId != changeEventArea.EventId)
			{
				throw new ArgumentException("Cannot change EventId.");
			}
		}

		private EventArea ConvertToEventArea(EventAreaDto area)
		{
			return new EventArea
			{
				Id = area.Id,
				LayoutId = area.LayoutId,
				EventId = area.EventId,
				Price = area.Price,
				Description = area.Description,
				CoordX = area.CoordX,
				CoordY = area.CoordY,
			};
		}

		private EventAreaDto ConvertToEventAreaDto(EventArea area)
		{
			return new EventAreaDto
			{
				Id = area.Id,
				LayoutId = area.LayoutId,
				EventId = area.EventId,
				Price = area.Price,
				Description = area.Description,
				CoordX = area.CoordX,
				CoordY = area.CoordY,
			};
		}
	}
}
