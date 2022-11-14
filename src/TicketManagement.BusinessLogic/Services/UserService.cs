using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.DataAccess.Interfaces.Repositories;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Services
{
	/// <summary>
	/// Represents operations for proxy user-repository calls with validation logic.
	/// </summary>
	internal class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;

		public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
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
		public async Task<IEnumerable<UserDto>> GetAllAsync(int roleId, int skip, int take)
		{
			var users = await _userRepository.GetAllAsync(roleId, skip, take);

			return users.AsEnumerable().Select(user => ConvertToUserDto(user));
		}

		/// <summary>
		/// Get collection users by this role.
		/// </summary>
		/// <param name="roleId">Id role.</param>
		/// <returns>Collection users.</returns>
		public async Task<IEnumerable<UserDto>> GetByRoleId(int roleId)
		{
			var users = await _userRepository.GetByRoleId(roleId);

			return users.AsEnumerable().Select(user => ConvertToUserDto(user));
		}

		/// <summary>
		/// Get user by his login from data set.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <returns>User.</returns>
		public async Task<UserDto> GetAsync(string login)
		{
			var user = await _userRepository.GetAsync(login);

			if (user == null)
			{
				return new UserDto();
			}

			return ConvertToUserDto(user);
		}

		/// <summary>
		/// Get user by his id from data set.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns>User.</returns>
		public async Task<UserDto> GetAsync(int id)
		{
			var user = await _userRepository.GetAsync(id);

			return ConvertToUserDto(user);
		}

		/// <summary>
		/// Add users in data set.
		/// </summary>
		/// <param name="entity">User to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> AddAsync(UserDto entity)
		{
			return await _userRepository.AddAsync(ConvertToUser(entity));
		}

		/// <summary>
		/// Delete users to data set.
		/// </summary>
		/// <param name="id">Id user to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Delete(int id)
		{
			return _userRepository.Delete(id);
		}

		/// <summary>
		/// Update users to data set.
		/// </summary>
		/// <param name="entity">User to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Update(UserDto entity)
		{
			return _userRepository.Update(ConvertToUser(entity));
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

		private User ConvertToUser(UserDto user)
		{
			return new User
			{
				Id = user.Id,
				Age = user.Age,
				Name = user.Name,
				Surname = user.Surname,
				Language = user.Language,
				UTC = user.UTC,
				Password = user.Password,
				Login = user.Login,
				Email = user.Email,
				Score = user.Score,
			};
		}

		private UserDto ConvertToUserDto(User user)
		{
			return new UserDto
			{
				Id = user.Id,
				Age = user.Age,
				Name = user.Name,
				Surname = user.Surname,
				Language = user.Language,
				UTC = user.UTC,
				Password = user.Password,
				Login = user.Login,
				Email = user.Email,
				Score = user.Score,
			};
		}
	}
}
