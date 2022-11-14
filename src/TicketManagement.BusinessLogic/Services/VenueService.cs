using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ValidationProviders;

namespace TicketManagement.BusinessLogic.Services
{
	/// <summary>
	/// Represents calling venue proxy repository with validation logic.
	/// </summary>
	internal class VenueService : IVenueService
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="VenueService"/> class.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public VenueService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Getting all venues from data set.
		/// In practice, there are not very many of them.
		/// </summary>
		/// <returns>Collection venues.</returns>
		public async Task<IEnumerable<VenueDto>> GetAllAsync()
		{
			var venues = await _unitOfWork.Venue.GetAllAsync();

			return venues.Select(venue => ConvertToVenueDto(venue)).AsEnumerable();
		}

		/// <summary>
		/// Getting venue from data set.
		/// </summary>
		/// <param name="parentId">Id venue.</param>
		/// <returns>Venue.</returns>
		public async Task<IEnumerable<VenueDto>> GetAllAsync(int parentId)
		{
			var venue = await GetAsync(parentId);
			return new List<VenueDto> { venue };
		}

		/// <summary>
		/// Get venue with validation logic.
		/// </summary>
		/// <returns>Venue.</returns>
		public async Task<VenueDto> GetAsync(int id)
		{
			var venue = await _unitOfWork.Venue.GetAsync(id);
			return ConvertToVenueDto(venue);
		}

		/// <summary>
		/// Method getting Venue by his unique name.
		/// </summary>
		/// <param name="name">Name venue.</param>
		/// <returns>Venue.</returns>
		public async Task<VenueDto> GetByNameAsync(string name)
		{
			var venue = await _unitOfWork.Venue.GetByNameAsync(name);
			if (venue == null)
			{
				return null;
			}
			return ConvertToVenueDto(venue);
		}

		/// <summary>
		/// Add venue with validation logic.
		/// Main validation - unique name venue.
		/// </summary>
		/// <param name="entity">Venue.</param>
		/// <returns>True - validation and adding successfull, false - successfull only validation.</returns>
		public async Task<bool> AddAsync(VenueDto entity)
		{
			var venue = ConvertToVenue(entity);
			VenueValidationProvider.IsValidToAdd(venue);

			// check unique name
			var unique = await _unitOfWork.Venue.AddAsync(venue);
			ExceptionIfNull(unique);

			return unique;
		}

		/// <summary>
		/// Delete venue by id with validation logic.
		/// </summary>
		/// <param name="id">Unique identifier venue.</param>
		/// <returns>True - validation and delete successfull, false - successfull only validation.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			VenueValidationProvider.IsValidToDelete(id);

			// ban on delete if venue have layout
			var countLayouts = await _unitOfWork.Layout.GetAllAsync(id);
			ExceptionIfNotEmpty(countLayouts);

			return await _unitOfWork.Venue.DeleteAsync(id);
		}

		/// <summary>
		/// Update venue with validation logic.
		/// Main validation - unique name venue.
		/// </summary>
		/// <param name="entity">Venue.</param>
		/// <returns>True - validation and updating successfull, false - successfull only validation.</returns>
		public async Task<bool> UpdateAsync(VenueDto entity)
		{
			var venue = ConvertToVenue(entity);
			VenueValidationProvider.IsValidToUpdate(venue);

			// check unique name
			var unique = await _unitOfWork.Venue.UpdateAsync(venue);
			ExceptionIfNull(unique);

			return unique;
		}

		private void ExceptionIfNull(bool flag)
		{
			if (!flag)
			{
				throw new InvalidOperationException("Invalid Name: Name does not repeat.");
			}
		}

		private void ExceptionIfNotEmpty(IQueryable<Layout> counts)
		{
			if (counts.Any())
			{
				throw new InvalidOperationException("Cannot delete venue when it is layout.");
			}
		}

		private Venue ConvertToVenue(VenueDto venue)
		{
			return new Venue
			{
				Id = venue.Id,
				Name = venue.Name,
				Description = venue.Description,
				Address = venue.Address,
				Phone = venue.Phone,
			};
		}

		private VenueDto ConvertToVenueDto(Venue venue)
		{
			return new VenueDto
			{
				Id = venue.Id,
				Name = venue.Name,
				Description = venue.Description,
				Address = venue.Address,
				Phone = venue.Phone,
			};
		}
	}
}
