using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.DataAccess.Entity
{
	/// <summary>
	/// provides access to User, Ticket and Role entities.
	/// </summary>
	internal class ApplicationDbContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
		/// </summary>
		/// <param name="options">Options for initializes.</param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ 
		}

		/// <summary>
		/// Set users in database.
		/// </summary>
		public DbSet<User> Users { get; set; }

		public DbSet<UserRole> UserRoles { get; set; }
		
		/// <summary>
		/// Set tickets in database.
		/// </summary>
		public DbSet<Ticket> Tickets { get; set; }
		
		/// <summary>
		/// Set roles in database.
		/// </summary>
		public DbSet<Role> Roles { get; set; }
	}
}
