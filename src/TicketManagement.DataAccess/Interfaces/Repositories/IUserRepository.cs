using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.DataAccess.Interfaces.Repositories
{
	/// <summary>
	/// Represent operations manipulate the user entity in data set.
	/// </summary>
	public interface IUserRepository
	{
		/// <summary>
		/// Getting count all users in data set.
		/// </summary>
		/// <returns>Count users in data set.</returns>
		Task<int> CountAsync(int roleId);

		/// <summary>
		/// Get users, the specified quantity and order.
		/// </summary>
		/// <param name="roleId">Type user-role.</param>
		/// <param name="skip">Number of missed users.</param>
		/// <param name="take">Number of taken users.</param>
		/// <returns>Collection users.</returns>
		Task<IQueryable<User>> GetAllAsync(int roleId, int skip, int take);

		/// <summary>
		/// Get collection users by this role.
		/// </summary>
		/// <param name="roleId">Id role.</param>
		/// <returns>Collection users.</returns>
		Task<IQueryable<User>> GetByRoleId(int roleId);

		/// <summary>
		/// Add users in data set.
		/// </summary>
		/// <param name="entity">User to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		Task<bool> AddAsync(User entity);

		/// <summary>
		/// Delete users to data set.
		/// </summary>
		/// <param name="id">Id user to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		bool Delete(int id);

		/// <summary>
		/// Get user by his id from data set.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns>User.</returns>
		Task<User> GetAsync(int id);

		/// <summary>
		/// Get user by his login from data set.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <returns>User.</returns>
		Task<User> GetAsync(string login);

		/// <summary>
		/// Adding specified user specified score.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <param name="score">Score to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		Task<bool> AddScore(int id, double score);

		/// <summary>
		/// Update users to data set.
		/// </summary>
		/// <param name="entity">User to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		bool Update(User entity);
	}
}
