using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.DataAccess.UnitOfWork;
using TicketManagement.EventAPI.Models;
using TicketManagement.DataAccess.Domain;
using TicketManagement.EventAPI.Services.Interfaces;

namespace TicketManagement.EventAPI.Services
{
	/// <summary>
	/// Provides implementation methods for getting various shapes venues and layouts.
	/// </summary>
	public class VenueLayoutGettingService : IVenueLayoutGettingService
	{
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="VenueLayoutGettingService"/> class.
		/// Create UnitOfWork-element.
		/// </summary>
		/// <param name="unitOfWork">Unit of work.</param>
		public VenueLayoutGettingService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		/// <summary>
		/// Getting all venues from data set.
		/// In practice, there are not very many of them.
		/// </summary>
		/// <returns>Collection venues.</returns>
		public async Task<IEnumerable<VenueDto>> GetAllVenuesAsync()
		{
			var venues = await _unitOfWork.Venue.GetAllAsync();

			return venues.Select(venue => ConvertToVenueDto(venue)).AsEnumerable();
		}

		/// <summary>
		/// Get collection layouts with validation logic.
		/// </summary>
		/// <returns>Collections layouts.</returns>
		public async Task<IEnumerable<LayoutDto>> GetAllLayoutsAsync(int venueId)
		{
			var layouts = await _unitOfWork.Layout.GetAllAsync(venueId);

			return layouts.Select(layout => ConvertToLayoutDto(layout)).AsEnumerable();
		}

		/// <summary>
		/// Get venue with validation logic.
		/// </summary>
		/// <returns>Venue.</returns>
		public async Task<VenueDto> GetVenueAsync(int id)
		{
			var venue = await _unitOfWork.Venue.GetAsync(id);
			if (venue == null)
			{
				return null;
			}
			return ConvertToVenueDto(venue);
		}

		/// <summary>
		/// Getting venue by his name.
		/// </summary>
		/// <param name="name">Name venue.</param>
		/// <returns>Venue.</returns>
		public async Task<VenueDto> GetVenueAsync(string name)
		{
			var venue = await _unitOfWork.Venue.GetByNameAsync(name);
			if (venue == null)
			{
				return null;
			}
			return ConvertToVenueDto(venue);
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