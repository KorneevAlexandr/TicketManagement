using Microsoft.EntityFrameworkCore;
using TicketManagement.UserAPI.Models;

namespace TicketManagement.UserAPI.DataContext
{
	public class UserContext : DbContext
	{
		public UserContext(DbContextOptions<UserContext> options)
			: base(options)
		{
		}

		public DbSet<UserRole> UserRoles { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Role> Roles { get; set; }

		public DbSet<Ticket> Tickets { get; set; }
	}
}
