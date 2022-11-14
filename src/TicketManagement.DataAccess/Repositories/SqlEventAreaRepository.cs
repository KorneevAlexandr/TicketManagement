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
	/// Represents access to the SQL database for event-areas getting.
	/// </summary>
	internal class SqlEventAreaRepository : IEventPlaceRepository<EventArea>, IDisposable, IAsyncDisposable
	{
		private readonly SqlConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlEventAreaRepository"/> class.
		/// </summary>
		public SqlEventAreaRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlEventAreaRepository"/> class.
		/// Create SqlConnection to database for manipulation.
		/// </summary>
		/// <param name="connectionString">Connection string to database.</param>
		public SqlEventAreaRepository(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
		}

		/// <summary>
		/// Getting a collection areas from SQL database, owned by event.
		/// </summary>
		/// <param name="id">Event.</param>
		/// <returns>Collection event-areas.</returns>		
		public async Task<IQueryable<EventArea>> GetPlacesAsync(int id)
		{
			var eventAreas = new List<EventArea>();

			var command = new SqlCommand("SELECT * FROM EventArea WHERE EventId = (@id)", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				eventAreas.Add(new EventArea
				{
					Id = Convert.ToInt32(reader["Id"]),
					EventId = Convert.ToInt32(reader["EventId"]),
					Description = reader["Description"].ToString(),
					CoordX = Convert.ToInt32(reader["CoordX"]),
					CoordY = Convert.ToInt32(reader["CoordY"]),
					Price = Convert.ToDecimal(reader["Price"]),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return eventAreas.AsQueryable();
		}

		/// <summary>
		/// Getting a collection places from a data set, owned be event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection seats or areas, owned by event.</returns>
		public async Task<IQueryable<EventArea>> GetPlacesByEventIdAsync(int id)
		{
			return await GetPlacesAsync(id);
		}

		/// <summary>
		/// Get one eventArea by id.
		/// </summary>
		/// <param name="id">Id eventArea.</param>
		/// <returns>EventArea.</returns>
		public async Task<EventArea> GetAsync(int id)
		{
			EventArea eventArea = null;

			var command = new SqlCommand("SELECT * FROM EventArea WHERE Id = (@id)", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				eventArea = new EventArea
				{
					Id = Convert.ToInt32(reader["Id"]),
					EventId = Convert.ToInt32(reader["EventId"]),
					Description = reader["Description"].ToString(),
					CoordX = Convert.ToInt32(reader["CoordX"]),
					CoordY = Convert.ToInt32(reader["CoordY"]),
					Price = Convert.ToDecimal(reader["Price"]),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return eventArea;
		}

		/// <summary>
		/// Delete a event-area from a SQL database.
		/// </summary>
		/// <param name="id">Id eventarea to delete.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Area id cannot be 0 or less.");
			}

			var command = new SqlCommand("DELETE FROM EventArea WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", id);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Update event-area in SQL database.
		/// </summary>
		/// <param name="entity">EventArea to update.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> UpdateAsync(EventArea entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not update null object!");
			}

			var command = new SqlCommand("UPDATE EventArea SET " +
				"Description = (@desc), CoordX = (@coordX), CoordY = (@coordY), Price = (@price) " +
				"WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", entity.Id);
			command.Parameters.AddWithValue("@desc", entity.Description);
			command.Parameters.AddWithValue("@coordX", entity.CoordX);
			command.Parameters.AddWithValue("@coordY", entity.CoordY);
			command.Parameters.AddWithValue("@price", entity.Price);

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
