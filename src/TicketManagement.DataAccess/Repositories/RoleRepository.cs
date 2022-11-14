using System.Threading.Tasks;
using TicketManagement.DataAccess.Entity;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.Repositories;

namespace TicketManagement.DataAccess.Repositories
{
	/// <summary>
	/// Represent implement operations getting the roles entity in data set.
	/// </summary>
	internal class RoleRepository : IRoleRepository
	{
		private readonly ApplicationDbContext _context;

		public RoleRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Getting role to data set.
		/// </summary>
		/// <param name="id">Id role to getting.</param>
		/// <returns>Role.</returns>
		public async Task<Role> GetAsync(int id)
		{
			return await _context.Roles.FindAsync(id);
		}
	}
}
