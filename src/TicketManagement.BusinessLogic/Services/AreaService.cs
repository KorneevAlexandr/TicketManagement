using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.UnitOfWork;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.ValidationProviders;

namespace TicketManagement.BusinessLogic.Services
{
	/// <summary>
	/// Represents calling area proxy repository with validation logic.
	/// </summary>
	internal class AreaService : IServiceBase<AreaDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="AreaService"/> class.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public AreaService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Get collection areas with validation logic.
		/// </summary>
		/// <returns>Collections areas.</returns>
		public async Task<IEnumerable<AreaDto>> GetAllAsync(int parentId)
		{
			var areas = await _unitOfWork.Area.GetAllAsync(parentId);
			return areas.Select(area => ConvertToAreaDto(area)).AsEnumerable();
		}

		/// <summary>
		/// Get element with validation logic.
		/// </summary>
		/// <param name="id">Id.</param>
		/// <returns>Area.</returns>
		public async Task<AreaDto> GetAsync(int id)
		{
			AreaValidationProvider.IsValidToDelete(id);
			var area = await _unitOfWork.Area.GetAsync(id);

			return ConvertToAreaDto(area);
		}

		/// <summary>
		/// Add area with validation logic.
		/// Main validation - unique description in one layout.
		/// </summary>
		/// <param name="entity">Area.</param>
		/// <returns>True - validation and adding successfull, false - successfull only validation.</returns>
		public async Task<bool> AddAsync(AreaDto entity)
		{
			var area = ConvertToArea(entity);
			AreaValidationProvider.IsValidToAdd(area);

			await CheckUniqueDescriptionInLayoutAsync(area);
			await CheckLayoutIdExistAsync(area);

			return await _unitOfWork.Area.AddAsync(area);
		}

		/// <summary>
		/// Delete area by id with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier seat.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			AreaValidationProvider.IsValidToDelete(id);

			var seats = await _unitOfWork.Seat.GetAllAsync(id);

			// cascade delete on BLL
			foreach (var seat in seats)
			{
				await _unitOfWork.Seat.DeleteAsync(seat.Id);
			}

			return await _unitOfWork.Area.DeleteAsync(id);
		}

		/// <summary>
		/// Update area with validation logic.
		/// Main validation - unique description in one layout.
		/// </summary>
		/// <param name="entity">Seat.</param>
		/// <returns>True - validation and updating successfull, false - successfull only validation.</returns>
		public async Task<bool> UpdateAsync(AreaDto entity)
		{
			var area = ConvertToArea(entity);

			AreaValidationProvider.IsValidToUpdate(area);

			await CheckUniqueDescriptionInLayoutAsync(area);
			await CheckLayoutIdExistAsync(area);
			await CheckChangeLayoutIdAsync(area);

			return await _unitOfWork.Area.UpdateAsync(area);
		}

		private async Task CheckUniqueDescriptionInLayoutAsync(Area area)
		{
			var areas = await _unitOfWork.Area.GetAllAsync(area.LayoutId);
			var otherDescriptions = areas.Where(a => a.Id != area.Id).Select(x => x.Description);

			if (otherDescriptions.Contains(area.Description.Trim()))
			{
				throw new InvalidOperationException("Invalid Description: Description does not repeat in this layout.");
			}
		}

		private async Task CheckLayoutIdExistAsync(Area area)
		{
			var result = await _unitOfWork.Layout.GetAsync(area.LayoutId) != null;
			if (!result)
			{
				throw new InvalidOperationException("Invalid LayoutId: such layout does not exist.");
			}
		}

		private async Task CheckChangeLayoutIdAsync(Area area)
		{
			var oldArea = await _unitOfWork.Area.GetAsync(area.Id);
			if (oldArea.LayoutId != area.LayoutId)
			{
				throw new InvalidOperationException("Invalid LayoutId: cannot change LayoutId.");
			}
		}

		private Area ConvertToArea(AreaDto area)
		{
			return new Area
			{
				Id = area.Id,
				LayoutId = area.LayoutId,
				Description = area.Description,
				CoordX = area.CoordX,
				CoordY = area.CoordY,
			};
		}

		private AreaDto ConvertToAreaDto(Area area)
		{
			return new AreaDto
			{
				Id = area.Id,
				LayoutId = area.LayoutId,
				Description = area.Description,
				CoordX = area.CoordX,
				CoordY = area.CoordY,
			};
		}
	}
}
