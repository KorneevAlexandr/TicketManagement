using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.UserAPI.DataContext;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Repositories.Interfaces;

namespace TicketManagement.UserAPI.Repositories
{
	/// <summary>
	/// Represent implement operations getting the roles entity in data set.
	/// </summary>
	public class RoleRepository : IRoleRepository
	{
		private readonly UserContext _context;

		public RoleRepository(UserContext context)
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

		public async Task<string> GetRole(int id)
		{
			var role = await _context.Roles.FirstAsync(role => role.Id == id);
			return role.Name;
		}
	}
}
