using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Entity;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.Repositories;

namespace TicketManagement.DataAccess.Repositories
{
	/// <summary>
	/// Represent implement operations manipulate the user entity in data set.
	/// </summary>
	internal class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Getting count all users in data set.
		/// </summary>
		/// <returns>Count users in data set.</returns>
		public async Task<int> CountAsync(int roleId)
		{
			int count = await _context.UserRoles.CountAsync(user => user.RoleId == roleId);
			return count;
		}

		/// <summary>
		/// Get users, the specified quantity and order.
		/// </summary>
		/// <param name="roleId">Type user-role.</param>
		/// <param name="skip">Number of missed users.</param>
		/// <param name="take">Number of taken users.</param>
		/// <returns>Collection users.</returns>
		public async Task<IQueryable<User>> GetAllAsync(int roleId, int skip, int take)
		{
			var usersRoles = _context.UserRoles.Where(role => role.Id == roleId).Select(role => role.UserId);
			var list = await _context.Users.Where(user => usersRoles.Contains(user.Id)).Skip(skip).Take(take).ToListAsync();
			return list.AsQueryable();
		}

		/// <summary>
		/// Get collection users by this role.
		/// </summary>
		/// <param name="roleId">Id role.</param>
		/// <returns>Collection users.</returns>
		public async Task<IQueryable<User>> GetByRoleId(int roleId)
		{
			var usersId = _context.UserRoles.Where(user => user.RoleId == roleId).Select(user => user.UserId);
			var list = await _context.Users.Where(user => usersId.Contains(user.Id)).ToListAsync();
			return list.AsQueryable();
		}

		/// <summary>
		/// Add users in data set.
		/// </summary>
		/// <param name="entity">User to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> AddAsync(User entity)
		{
			await _context.Users.AddAsync(entity);
			SaveChangeAsync();

			return true;
		}

		/// <summary>
		/// Delete users to data set.
		/// </summary>
		/// <param name="id">Id user to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Delete(int id)
		{
			if (id <= 0)
			{
				return false;
			}

			_context.Users.Remove(new User { Id = id });
			SaveChangeAsync();
			return true;
		}

		/// <summary>
		/// Get user by his id from data set.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns>User.</returns>
		public async Task<User> GetAsync(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		/// <summary>
		/// Get user by his login from data set.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <returns>User.</returns>
		public async Task<User> GetAsync(string login)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
		}

		/// <summary>
		/// Adding specified user specified score.
		/// </summary>
		/// <param name="id">User id.</param>
		/// <param name="score">Score to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> AddScore(int id, double score)
		{
			var user = await GetAsync(id);

			user.Score += score;
			_context.Update(user);

			SaveChangeAsync();
			return true;
		}

		/// <summary>
		/// Update users to data set.
		/// </summary>
		/// <param name="entity">User to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Update(User entity)
		{
			if (entity == null)
			{
				return false;
			}

			var user = GetAsync(entity.Id);
			user.Result.Name = entity.Name ?? user.Result.Name;
			user.Result.Surname = entity.Surname ?? user.Result.Surname;
			user.Result.Login = entity.Login ?? user.Result.Login;
			user.Result.Password = entity.Password ?? user.Result.Password;
			user.Result.Age = entity.Age == 0 ? user.Result.Age : entity.Age;
			user.Result.Email = entity.Email ?? user.Result.Email;
			user.Result.UTC = entity.UTC == DateTimeOffset.MinValue ? user.Result.UTC : entity.UTC;
			user.Result.Language = entity.Language ?? user.Result.Language;
			user.Result.Score = entity.Score;

			SaveChangeAsync();
			return true;
		}

		public async Task UpdateRole(int userId, int roleId)
		{
			var user = await _context.UserRoles.FirstOrDefaultAsync(user => user.Id == userId);
			user.RoleId = roleId;
			SaveChangeAsync();
		}

		private async void SaveChangeAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
