using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Repositories;
using TicketManagement.UserAPI.Repositories.Interfaces;
using TicketManagement.UserAPI.Services.Interfaces;

namespace TicketManagement.UserAPI.Services
{
	/// <summary>
	/// Represents operations for proxy user-repository calls with validation logic.
	/// </summary>
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly ITicketRepository _ticketRepository;

		public UserService(IUserRepository userRepository, IRoleRepository roleRepository, ITicketRepository ticketRepository)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_ticketRepository = ticketRepository;
		}

		/// <summary>
		/// Getting count all users in data set.
		/// </summary>
		/// <returns>Count users in data set.</returns>
		public async Task<int> CountAsync(int roleId)
		{
			return await _userRepository.CountAsync(roleId);
		}

		/// <summary>
		/// Get users, the specified quantity and order.
		/// </summary>
		/// <param name="roleId">Type user-role.</param>
		/// <param name="skip">Number of missed users.</param>
		/// <param name="take">Number of taken users.</param>
		/// <returns>Collection users.</returns>
		public async Task<IEnumerable<User>> GetAllAsync(int roleId, int skip, int take)
		{
			var users = await _userRepository.GetAllAsync(roleId, skip, take);
			return users.AsEnumerable();
		}

		/// <summary>
		/// Get collection users by this role.
		/// </summary>
		/// <param name="roleId">Id role.</param>
		/// <returns>Collection users.</returns>
		public async Task<IEnumerable<User>> GetByRoleId(int roleId)
		{
			var users = await _userRepository.GetByRoleId(roleId);

			return users.AsEnumerable();
		}

		/// <summary>
		/// Get user by his login from data set.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <returns>User.</returns>
		public async Task<User> GetAsync(string login)
		{
			var user = await _userRepository.GetAsync(login);
			return user;
		}

		/// <summary>
		/// Get user by his id from data set.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns>User.</returns>
		public async Task<User> GetAsync(int id)
		{
			return await _userRepository.GetAsync(id);
		}

		/// <summary>
		/// Add users in data set.
		/// </summary>
		/// <param name="entity">User to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> AddAsync(User entity)
		{
			await _userRepository.AddAsync(entity);
			var user = await _userRepository.GetAsync(entity.Login);
			await _userRepository.AddRole(user);
			return true;
		}

		/// <summary>
		/// Delete users to data set.
		/// </summary>
		/// <param name="id">Id user to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> Delete(int id)
		{
			return await _userRepository.Delete(id);
		}

		/// <summary>
		/// Update users to data set.
		/// </summary>
		/// <param name="entity">User to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> Update(User entity)
		{
			return await _userRepository.Update(entity);
		}

		public async Task UpdateRole(int userId, int roleId)
		{
			await _userRepository.UpdateRole(userId, roleId);
		}

		/// <summary>
		/// Adding specified user specified score.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <param name="score">Score to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> AddScore(int id, double score)
		{
			return await _userRepository.AddScore(id, score);
		}

		public async Task<string> GetRole(int roleId)
		{
			return await _roleRepository.GetRole(roleId);
		}

		public async Task<Role[]> GetUserRoles(int userId)
		{
			var roles = await _userRepository.GetUserRoles(userId);
			return roles;
		}

		public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(int userId)
		{
			return await _ticketRepository.GetAllAsync(userId);
		}
	}
}
