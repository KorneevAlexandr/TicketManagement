using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.DataAccess.Domain;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ValidationProviders;

namespace TicketManagement.BusinessLogic.Services
{
	/// <summary>
	/// Represents calling layout proxy repository with validation logic.
	/// </summary>
	internal class LayoutService : IServiceBase<LayoutDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="LayoutService"/> class.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public LayoutService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Get collection layouts with validation logic.
		/// </summary>
		/// <returns>Collections layouts.</returns>
		public async Task<IEnumerable<LayoutDto>> GetAllAsync(int parentId)
		{
			var layouts = await _unitOfWork.Layout.GetAllAsync(parentId);

			return layouts.Select(layout => ConvertToLayoutDto(layout)).AsEnumerable();
		}

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>Layout.</returns>
		public async Task<LayoutDto> GetAsync(int id)
		{
			LayoutValidationProvider.IsValidToDelete(id);

			var layout = await _unitOfWork.Layout.GetAsync(id);
			return ConvertToLayoutDto(layout);
		}

		/// <summary>
		/// Add layout with validation logic.
		/// Main validation - unique name in one venue.
		/// </summary>
		/// <param name="entity">Layout.</param>
		/// <returns>True - validation and adding successfull, false - successfull only validation.</returns>
		public async Task<bool> AddAsync(LayoutDto entity)
		{
			var layout = ConvertToLayout(entity);
			LayoutValidationProvider.IsValidToAdd(layout);

			await CheckVenueIdExistAsync(layout);

			// check unique name in venue
			await CheckUniqueNameLayoutAddAsync(layout);

			return await _unitOfWork.Layout.AddAsync(layout);
		}

		/// <summary>
		/// Delete layout by id with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier layout.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			LayoutValidationProvider.IsValidToDelete(id);

			await CheckNotDeleteEventAsync(id);

			// cascade delete area-seat
			var areas = await _unitOfWork.Area.GetAllAsync(id);
			var seats = new List<Seat>();
			foreach (var area in areas)
			{
				seats.AddRange(await _unitOfWork.Seat.GetAllAsync(area.Id));
			}

			foreach (var seat in seats)
			{
				await _unitOfWork.Seat.DeleteAsync(seat.Id);
			}
			foreach (var area in areas)
			{
				await _unitOfWork.Area.DeleteAsync(area.Id);
			}

			return await _unitOfWork.Layout.DeleteAsync(id);
		}

		/// <summary>
		/// Update layout with validation logic.
		/// Main validation - unique name in one venue.
		/// </summary>
		/// <param name="entity">Layout.</param>
		/// <returns>True - validation and updating successfull, false - successfull only validation.</returns>
		public async Task<bool> UpdateAsync(LayoutDto entity)
		{
			var layout = ConvertToLayout(entity);
			LayoutValidationProvider.IsValidToUpdate(layout);

			await CheckVenueIdExistAsync(layout);

			// check unique name in venue
			await CheckUniqueNameLayoutUpdateAsync(layout);
			await CheckChangeVenueIdAsync(layout);

			return await _unitOfWork.Layout.UpdateAsync(layout);
		}

		private async Task CheckNotDeleteEventAsync(int id)
		{
			var layout = await _unitOfWork.Layout.GetAsync(id);
			var events = await _unitOfWork.Event.GetAllAsync(layout.VenueId);
			var countEvents = events.Where(x => x.LayoutId == id);

			if (countEvents.Any())
			{
				throw new InvalidOperationException("Cannot delete layout when it is events.");
			}
		}

		private async Task CheckChangeVenueIdAsync(Layout layout)
		{
			var oldLayout = await _unitOfWork.Layout.GetAsync(layout.Id);
			if (oldLayout != null && oldLayout.VenueId != layout.VenueId)
			{
				throw new InvalidOperationException("Invalid VenueId: cannot change VenueId.");
			}
		}

		private async Task CheckVenueIdExistAsync(Layout layout)
		{
			var venue = await _unitOfWork.Venue.GetAsync(layout.VenueId);
			if (venue == null)
			{
				throw new InvalidOperationException("Invalid VenueId: such venue does not exist.");
			}
		}

		private async Task CheckUniqueNameLayoutAddAsync(Layout layout)
		{
			var layouts = await _unitOfWork.Layout.GetAllAsync(layout.VenueId);
			var otherNames = layouts.Select(x => x.Name.Trim());

			if (otherNames.Contains(layout.Name.Trim()))
			{
				throw new InvalidOperationException("Invalid Name: Name does not repeat in this venue.");
			}
		}

		private async Task CheckUniqueNameLayoutUpdateAsync(Layout layout)
		{
			var layouts = await _unitOfWork.Layout.GetAllAsync(layout.VenueId);
			var otherNames = layouts.Where(l => l.Id != layout.Id)
							 .Select(x => x.Name.Trim());

			if (otherNames.Contains(layout.Name.Trim()))
			{
				throw new InvalidOperationException("Invalid Name: Name does not repeat in this venue.");
			}
		}

		private Layout ConvertToLayout(LayoutDto layout)
		{
			return new Layout
			{
				Id = layout.Id,
				Description = layout.Description,
				VenueId = layout.VenueId,
				Name = layout.Name,
			};
		}

		private LayoutDto ConvertToLayoutDto(Layout layout)
		{
			return new LayoutDto
			{
				Id = layout.Id,
				Description = layout.Description,
				VenueId = layout.VenueId,
				Name = layout.Name,
			};
		}
	}
}
