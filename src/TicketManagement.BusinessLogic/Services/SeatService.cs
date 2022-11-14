using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ValidationProviders;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Services
{
	/// <summary>
	/// Represents calling seat proxy repository with validation logic.
	/// </summary>
	internal class SeatService : IServiceBase<SeatDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="SeatService"/> class.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public SeatService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Get collection seats with validation logic.
		/// </summary>
		/// <returns>Collections seats.</returns>
		public async Task<IEnumerable<SeatDto>> GetAllAsync(int parentId)
		{
			var seats = await _unitOfWork.Seat.GetAllAsync(parentId);

			return seats.Select(seat => ConvertToSeatDto(seat)).AsEnumerable();
		}

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>Seat.</returns>
		public async Task<SeatDto> GetAsync(int id)
		{
			SeatValidationProvider.IsValidToDelete(id);

			var seat = await _unitOfWork.Seat.GetAsync(id);
			return ConvertToSeatDto(seat);
		}

		/// <summary>
		/// Add seat with validation logic.
		/// Main validation - unique row and number in one area.
		/// </summary>
		/// <param name="entity">Seat.</param>
		/// <returns>True - validation and adding successfull, false - successfull only validation.</returns>
		public async Task<bool> AddAsync(SeatDto entity)
		{
			var seat = ConvertToSeat(entity);
			SeatValidationProvider.IsValidToAdd(seat);

			await CheckAreaIdExistAsync(seat);
			await CheckSimilarSeatAddAsync(seat);

			return await _unitOfWork.Seat.AddAsync(seat);
		}

		/// <summary>
		/// Delete seat by id with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier seat.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			SeatValidationProvider.IsValidToDelete(id);

			return await _unitOfWork.Seat.DeleteAsync(id);
		}

		/// <summary>
		/// Update seat with validation logic.
		/// Main validation - unique row and number in one area (when update areaId).
		/// </summary>
		/// <param name="entity">Seat.</param>
		/// <returns>True - validation and updating successfull, false - successfull only validation.</returns>
		public async Task<bool> UpdateAsync(SeatDto entity)
		{
			var seat = ConvertToSeat(entity);
			SeatValidationProvider.IsValidToUpdate(seat);

			await CheckAreaIdExistAsync(seat);
			await CheckSimilarSeatUpdateAsync(seat);

			return await _unitOfWork.Seat.UpdateAsync(seat);
		}

		private async Task CheckAreaIdExistAsync(Seat seat)
		{
			var result = await _unitOfWork.Area.GetAsync(seat.AreaId) != null;
			if (!result)
			{
				throw new InvalidOperationException("Invalid AreaId: such area does not exist.");
			}
		}

		private async Task CheckSimilarSeatAddAsync(Seat seat)
		{
			var seats = await _unitOfWork.Seat.GetAllAsync(seat.AreaId);
			var similarSeats = seats.Select(x => new int[2] { x.Row, x.Number });
			var similarCount = similarSeats.Where(a => a[0] == seat.Row && a[1] == seat.Number);
			if (similarCount.Any())
			{
				throw new InvalidOperationException("Seat with the same location there is.");
			}
		}

		private async Task CheckSimilarSeatUpdateAsync(Seat seat)
		{
			var seats = await _unitOfWork.Seat.GetAllAsync(seat.AreaId);
			var similarSeats = seats.Where(s => s.Id != seat.Id)
				   .Select(x => new int[2] { x.Row, x.Number });
			var similarCount = similarSeats.Where(a => a[0] == seat.Row && a[1] == seat.Number);
			if (similarCount.Any())
			{
				throw new InvalidOperationException("Seat with the same location there is.");
			}
		}

		private Seat ConvertToSeat(SeatDto seat)
		{
			return new Seat
			{
				Id = seat.Id,
				AreaId = seat.AreaId,
				Number = seat.Number,
				Row = seat.Row,
			};
		}

		private SeatDto ConvertToSeatDto(Seat seat)
		{
			return new SeatDto
			{
				Id = seat.Id,
				AreaId = seat.AreaId,
				Number = seat.Number,
				Row = seat.Row,
			};
		}
	}
}
