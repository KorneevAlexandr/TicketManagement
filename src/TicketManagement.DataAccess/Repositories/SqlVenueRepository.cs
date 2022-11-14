using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.Repositories;
using System.Data.SqlClient;

namespace TicketManagement.DataAccess.Repositories
{
	/// <summary>
	/// Represents access to the SQL database for seats manipulation.
	/// </summary>
	internal class SqlVenueRepository : IVenueRepository, IDisposable, IAsyncDisposable
	{
		private readonly SqlConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlVenueRepository"/> class.
		/// </summary>
		public SqlVenueRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlVenueRepository"/> class.
		/// Create SqlConnection to database for manipulation.
		/// </summary>
		/// <param name="connectionString">Connection string to database.</param>
		public SqlVenueRepository(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
		}

		/// <summary>
		/// Getting all venues from data set.
		/// In practice, there are not very many of them.
		/// </summary>
		/// <returns>Collection venues.</returns>
		public async Task<IQueryable<Venue>> GetAllAsync()
		{
			var venues = new List<Venue>();

			var command = new SqlCommand("SELECT * FROM Venue", _connection);
			var reader = await command.ExecuteReaderAsync();

			while (await reader.ReadAsync())
			{
				venues.Add(new Venue
				{
					Id = Convert.ToInt32(reader["Id"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
					Address = reader["Address"].ToString(),
					Phone = reader["Phone"].ToString(),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return venues.AsQueryable();
		}

		/// <summary>
		/// Getting a collection venues from SQL database.
		/// </summary>
		/// <returns>Collection venues.</returns>
		public async Task<IQueryable<Venue>> GetAllAsync(int parentId)
		{
			var venue = await GetAsync(parentId);
			var list = new List<Venue> { venue };
			return list.AsQueryable();
		}

		/// <summary>
		/// Get one venue by id.
		/// </summary>
		/// <param name="id">Id venue.</param>
		/// <returns>Venue.</returns>
		public async Task<Venue> GetAsync(int id)
		{
			Venue venue = null;

			var command = new SqlCommand("SELECT * FROM Venue WHERE Id = @id", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				venue = new Venue
				{
					Id = Convert.ToInt32(reader["Id"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
					Address = reader["Address"].ToString(),
					Phone = reader["Phone"].ToString(),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return venue;
		}

		/// <summary>
		/// Getting Venue by his unique name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Venue.</returns>
		public async Task<Venue> GetByNameAsync(string name)
		{
			Venue venue = null;

			var command = new SqlCommand("SELECT * FROM Venue WHERE Name = @name", _connection);
			command.Parameters.AddWithValue("@name", name);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				venue = new Venue
				{
					Id = Convert.ToInt32(reader["Id"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
					Address = reader["Address"].ToString(),
					Phone = reader["Phone"].ToString(),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return venue;
		}

		/// <summary>
		/// Adding venue to SQL database.
		/// </summary>
		/// <param name="entity">Venue to add.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> AddAsync(Venue entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not add null object!");
			}

			var command = new SqlCommand("INSERT INTO Venue (Description, Address, Phone, Name) " +
				"VALUES (@desc, @address, @phone, @name)", _connection);

			command.Parameters.AddWithValue("@desc", entity.Description);
			command.Parameters.AddWithValue("@address", entity.Address);
			command.Parameters.AddWithValue("@phone", entity.Phone);
			command.Parameters.AddWithValue("@name", entity.Name);

			int changeRow = 0;
			try
			{
				changeRow = await command.ExecuteNonQueryAsync();
			}
			catch
			{
				changeRow = -1;
			}
			finally
			{
				await command.DisposeAsync();
			}

			return changeRow > 0;
		}

		/// <summary>
		/// Delete a venue from a SQL database.
		/// </summary>
		/// <param name="id">Id venue to delete.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Venue id cannot be 0 or less.");
			}

			var command = new SqlCommand("DELETE FROM Venue WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", id);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Update venue in SQL database.
		/// </summary>
		/// <param name="entity">Venue to update.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> UpdateAsync(Venue entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not update null object!");
			}

			var command = new SqlCommand("UPDATE Venue SET " +
				"Description = (@desc), Address = (@address), Phone = (@phone), Name = (@name) " +
				"WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@Id", entity.Id);
			command.Parameters.AddWithValue("@desc", entity.Description);
			command.Parameters.AddWithValue("@address", entity.Address);
			command.Parameters.AddWithValue("@phone", entity.Phone);
			command.Parameters.AddWithValue("@name", entity.Name);

			int changeRow = 0;
			try
			{
				changeRow = await command.ExecuteNonQueryAsync();
			}
			catch
			{
				changeRow = -1;
			}
			finally
			{
				await command.DisposeAsync();
			}

			return changeRow > 0;
		}

		/// <summary>
		/// Virtual method dispose for clearing object.
		/// </summary>
		/// <param name="disposing">If get true - start clearing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				_connection.Dispose();
		}

		/// <summary>
		/// Start clearing.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Method for asynchronous clearing.
		/// </summary>
		/// <returns>Task.</returns>
		public async ValueTask DisposeAsync()
		{
			await DisposeAsyncCore();

			Dispose(false);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Protected method for asynchronous clearing.
		/// </summary>
		/// <returns>Task.</returns>
		protected virtual async ValueTask DisposeAsyncCore()
		{
			if (_connection != null)
			{
				await _connection.DisposeAsync().ConfigureAwait(false);
			}
		}
	}
}
