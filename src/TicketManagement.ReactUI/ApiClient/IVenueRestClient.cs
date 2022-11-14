using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Layouts;
using TicketManagement.ClientModels.Venues;

namespace TicketManagement.ReactUI.ApiClient
{
	public interface IVenueRestClient
	{
		[Get("venueManagement/Venues")]
		Task<List<VenueModel>> GetVenues([Header("Token")] string token);

		[Get("layoutManagement/Layouts")]
		Task<LayoutModel> GetLayouts(int venueId, [Header("Token")] string token);
	}
}
