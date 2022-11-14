using RestEase;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Areas;
using TicketManagement.ClientModels.Layouts;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.WebApplication.Clients
{
	/// <summary>
	/// A client interface that provides methods for calling methods of the venue manipulating service.
	/// </summary>
	public interface IVenueRestClient
	{
		[Get("venueManagement/Venues")]
		Task<List<VenueModel>> GetVenues([Header("Token")] string token);

		[Get("venueManagement/Venue")]
		Task<VenueModel> GetVenue(int venueId, [Header("Token")] string token);

		[Post("venueManagement/Venue")]
		Task CreateVenue([Body] VenueModel model, [Header("Token")] string token);

		[Put("venueManagement/Venue")]
		Task UpdateVenue([Body] VenueModel model, [Header("Token")] string token);

		[Delete("venueManagement/Venue")]
		Task DeleteVenue(int venueId, [Header("Token")] string token);

		[Get("layoutManagement/Layouts")]
		Task<LayoutModel> GetLayouts(int venueId, [Header("Token")] string token);

		[Get("layoutManagement/Layout")]
		Task<LayoutFullInfoModel> GetLayout(int layoutId, [Header("Token")] string token);

		[Post("layoutManagement/Layout")]
		Task CreateLayout([Body] CreateLayoutModel model, [Header("Token")] string token);

		[Delete("layoutManagement/Layout")]
		Task DeleteLayout(int layoutId, [Header("Token")] string token);

		[Put("layoutManagement/Layout")]
		Task UpdateLayout([Body] LayoutInfoModel model, [Header("Token")] string token);

		[Get("areaManagement/Areas")]
		Task<List<AreaInfoModel>> GetAreas(int layoutId, [Header("Token")] string token);

		[Get("areaManagement/Area")]
		Task<AreaInfoModel> GetArea(int areaId, [Header("Token")] string token);

		[Post("areaManagement/Area")]
		Task CreateArea([Body] AreaInfoModel model, [Header("Token")] string token);

		[Delete("areaManagement/Area")]
		Task DeleteArea(int areaId, [Header("Token")] string token);

		[Put("areaManagement/Area")]
		Task UpdateArea([Body] AreaInfoModel model, [Header("Token")] string token);
	}
}
