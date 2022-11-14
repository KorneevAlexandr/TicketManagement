using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ValidationProviders;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.BusinessLogic.Services
{
	/// <summary>
	/// Represents calling eventSeat proxy repository with validation logic.
	/// </summary>
	internal class EventSeatService : IEventPlaceService<EventSeatDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="EventSeatService"/> class.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public EventSeatService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>EventSeat.</returns>
		public async Task<EventSeatDto> GetAsync(int id)
		{
			EventValidationProvider.IsValidEventToPlaces(id);
			var eventSeat = await _unitOfWork.EventSeat.GetAsync(id);

			return ConvertToEventSeatDto(eventSeat);
		}

		/// <summary>
		/// Get collection event-seats with validation logic.
		/// </summary>
		/// <returns>Collections events.</returns>
		public async Task<IEnumerable<EventSeatDto>> GetPlacesAsync(int id)
		{
			EventValidationProvider.IsValidEventToPlaces(id);

			var eventSeats = await _unitOfWork.EventSeat.GetPlacesAsync(id);
			return eventSeats.Select(seat => ConvertToEventSeatDto(seat)).AsEnumerable();
		}

		/// <summary>
		/// Getting a collection places from a data set, owned be event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection seats, owned by event.</returns>
		public async Task<IEnumerable<EventSeatDto>> GetPlacesByEventIdAsync(int id)
		{
			EventValidationProvider.IsValidEventToPlaces(id);

			var eventSeats = await _unitOfWork.EventSeat.GetPlacesByEventIdAsync(id);
			return eventSeats.Select(seat => ConvertToEventSeatDto(seat)).AsEnumerable();
		}

		/// <summary>
		/// Delete event-seat by id with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier event.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			EventSeatValidationProvider.IsValidToDelete(id);
			var eventSeat = await _unitOfWork.EventSeat.GetAsync(id);
			
			return await _unitOfWork.EventSeat.DeleteAsync(eventSeat.Id);
		}

		/// <summary>
		/// Update event-seat with validation logic.
		/// Main validation - can't be created without any seats.
		/// </summary>
		/// <param name="entity">EventArea.</param>
		/// <returns>True - validation and updating successfull, false - successfull only validation.</returns>
		public async Task<bool> UpdateAsync(EventSeatDto entity)
		{
			var eventSeat = ConvertToEventSeat(entity);
			EventSeatValidationProvider.IsValidToUpdate(eventSeat);

			var comparerEventSeat = await _unitOfWork.EventSeat.GetAsync(eventSeat.Id);
			ExceptionIfNotEqualId(eventSeat, comparerEventSeat);

			return await _unitOfWork.EventSeat.UpdateAsync(eventSeat);
		}

		private void ExceptionIfNotEqualId(EventSeat eventSeat, EventSeat comparerEventSeat)
		{
			if (eventSeat.EventAreaId != comparerEventSeat.EventAreaId)
			{
				throw new ArgumentException("Cannot change EventAreaId.");
			}
		}

		private EventSeat ConvertToEventSeat(EventSeatDto seat)
		{
			return new EventSeat
			{
				Id = seat.Id,
				AreaId = seat.AreaId,
				Number = seat.Number,
				Row = seat.Row,
				EventAreaId = seat.EventAreaId,
				State = (SeatState)seat.State,
			};
		}

		private EventSeatDto ConvertToEventSeatDto(EventSeat seat)
		{
			return new EventSeatDto
			{
				Id = seat.Id,
				AreaId = seat.AreaId,
				Number = seat.Number,
				Row = seat.Row,
				EventAreaId = seat.EventAreaId,
				State = (SeatStateDto)seat.State,
			};
		}
	}
}
