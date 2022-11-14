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
	/// Represents access to the SQL database for layout manipulation.
	/// </summary>
	internal class SqlLayoutRepository : IBaseRepository<Layout>, IDisposable, IAsyncDisposable
	{
		private readonly SqlConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlLayoutRepository"/> class.
		/// </summary>
		public SqlLayoutRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlLayoutRepository"/> class.
		/// Create SqlConnection to database for manipulation.
		/// </summary>
		/// <param name="connectionString">Connection string to database.</param>
		public SqlLayoutRepository(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
		}

		/// <summary>
		/// Getting a collection layouts from SQL database.
		/// </summary>
		/// <returns>Collection layouts.</returns>
		public async Task<IQueryable<Layout>> GetAllAsync(int parentId)
		{
			var layouts = new List<Layout>();

			var command = new SqlCommand("SELECT * FROM Layout WHERE VenueId = @id", _connection);
			command.Parameters.AddWithValue("@id", parentId);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				layouts.Add(new Layout
				{
					Id = Convert.ToInt32(reader["Id"]),
					VenueId = Convert.ToInt32(reader["VenueId"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return layouts.AsQueryable();
		}

		/// <summary>
		/// Get one layout by id.
		/// </summary>
		/// <param name="id">Id layout.</param>
		/// <returns>Layout.</returns>
		public async Task<Layout> GetAsync(int id)
		{
			Layout layout = null;

			var command = new SqlCommand("SELECT * FROM Layout WHERE Id = @id", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				layout = new Layout
				{
					Id = Convert.ToInt32(reader["Id"]),
					VenueId = Convert.ToInt32(reader["VenueId"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return layout;
		}

		/// <summary>
		/// Adding layout to SQL database.
		/// </summary>
		/// <param name="entity">Layout to add.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> AddAsync(Layout entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not add null object!");
			}

			var command = new SqlCommand("INSERT INTO Layout (VenueId, Description, Name) " +
				"VALUES (@venueId, @desc, @name)", _connection);

			command.Parameters.AddWithValue("@venueId", entity.VenueId);
			command.Parameters.AddWithValue("@desc", entity.Description);
			command.Parameters.AddWithValue("@name", entity.Name);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Delete a layout from a SQL database.
		/// </summary>
		/// <param name="id">Id layout to delete.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Layout id cannot be 0 or less.");
			}

			var command = new SqlCommand("DELETE FROM Layout WHERE Id = (@id)", _connection);
			command.Parameters.AddWithValue("@id", id);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Update layout in SQL database.
		/// </summary>
		/// <param name="entity">Layout to update.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> UpdateAsync(Layout entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not update null object!");
			}

			var command = new SqlCommand("UPDATE Layout SET " +
				"VenueId = (@venueId), Description = (@desc), Name = (@name) " +
				"WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", entity.Id);
			command.Parameters.AddWithValue("@venueId", entity.VenueId);
			command.Parameters.AddWithValue("@desc", entity.Description);
			command.Parameters.AddWithValue("@name", entity.Name);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
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
