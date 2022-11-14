using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.BLInterfaces
{
	/// <summary>
	/// Represents operations for proxy user-repository calls with validation logic.
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		/// Getting count all users in data set.
		/// </summary>
		/// <returns>Count users in data set.</returns>
		Task<int> CountAsync(int roleId);

		/// <summary>
		/// Get user by his login from data set.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <returns>User.</returns>
		Task<UserDto> GetAsync(string login);

		/// <summary>
		/// Get user by his id from data set.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns>User.</returns>
		Task<UserDto> GetAsync(int id);

		/// <summary>
		/// Get users, the specified quantity and order.
		/// </summary>
		/// <param name="roleId">Type user-role.</param>
		/// <param name="skip">Number of missed users.</param>
		/// <param name="take">Number of taken users.</param>
		/// <returns>Collection users.</returns>
		Task<IEnumerable<UserDto>> GetAllAsync(int roleId, int skip, int take);

		/// <summary>
		/// Get collection users by this role.
		/// </summary>
		/// <param name="roleId">Id role.</param>
		/// <returns>Collection users.</returns>
		Task<IEnumerable<UserDto>> GetByRoleId(int roleId);

		/// <summary>
		/// Adding specified user specified score.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <param name="score">Score to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		Task<bool> AddScore(int id, double score);

		/// <summary>
		/// Add users in data set.
		/// </summary>
		/// <param name="entity">User to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		Task<bool> AddAsync(UserDto entity);

		/// <summary>
		/// Delete users to data set.
		/// </summary>
		/// <param name="id">Id user to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		bool Delete(int id);

		/// <summary>
		/// Update users to data set.
		/// </summary>
		/// <param name="entity">User to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		bool Update(UserDto entity);
	}
}
