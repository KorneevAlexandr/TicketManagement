using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Extensions;

namespace TicketManagement.BusinessLogic.Extensions
{
	public static class ServicesInjectExtensions
	{
		public static IServiceCollection InjectServicesBLL(this IServiceCollection services, string connectionString)
		{
			services.InjectServicesDAL(connectionString);

			services.AddScoped<IVenueService, VenueService>();
			services.AddScoped<IServiceBase<LayoutDto>, LayoutService>();
			services.AddScoped<IServiceBase<AreaDto>, AreaService>();
			services.AddScoped<IServiceBase<SeatDto>, SeatService>();
			services.AddScoped<IEventService, EventService>();
			services.AddScoped<IEventPlaceService<EventAreaDto>, EventAreaService>();
			services.AddScoped<IEventPlaceService<EventSeatDto>, EventSeatService>();
			services.AddScoped<ITicketService, TicketService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IThirdPartyEventService, ThirdPartyEventService>();

			return services;
		}
	}
}
