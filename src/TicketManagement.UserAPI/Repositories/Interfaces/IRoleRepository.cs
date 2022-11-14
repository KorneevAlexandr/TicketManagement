using System.Threading.Tasks;
using TicketManagement.UserAPI.Models;

namespace TicketManagement.UserAPI.Repositories.Interfaces
{
	/// <summary>
	/// Represent operations getting the roles entity in data set.
	/// </summary>
	public interface IRoleRepository
	{
		/// <summary>
		/// Getting role to data set.
		/// </summary>
		/// <param name="id">Id role to getting.</param>
		/// <returns>Role.</returns>
		Task<Role> GetAsync(int id);

		Task<string> GetRole(int id);
	}
}
