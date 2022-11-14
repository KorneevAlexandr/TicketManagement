using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Entity;
using TicketManagement.DataAccess.Interfaces.Repositories;
using TicketManagement.DataAccess.Interfaces.UnitOfWork;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.DataAccess.Extensions
{
	public static class RepositoriesInjectExtensions
	{
		public static IServiceCollection InjectServicesDAL(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<ITicketRepository, TicketRepository>();
			services.AddScoped<IUserRepository, UserRepository>();

			services.AddScoped<IVenueRepository, SqlVenueRepository>(options => new SqlVenueRepository(connectionString));
			services.AddScoped<IEventRepository, SqlEventRepository>(options => new SqlEventRepository(connectionString));
			services.AddScoped<IBaseRepository<Layout>, SqlLayoutRepository>(options => new SqlLayoutRepository(connectionString));
			services.AddScoped<IBaseRepository<Area>, SqlAreaRepository>(options => new SqlAreaRepository(connectionString));
			services.AddScoped<IBaseRepository<Seat>, SqlSeatRepository>(options => new SqlSeatRepository(connectionString));
			services.AddScoped<IEventPlaceRepository<EventArea>, SqlEventAreaRepository>(options => new SqlEventAreaRepository(connectionString));
			services.AddScoped<IEventPlaceRepository<EventSeat>, SqlEventSeatRepository>(options => new SqlEventSeatRepository(connectionString));
			services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

			return services;
		}
	}
}
