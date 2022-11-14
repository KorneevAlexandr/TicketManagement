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
	/// Represents access to the SQL database for events manipulation.
	/// </summary>
	internal class SqlEventRepository : IEventRepository, IDisposable, IAsyncDisposable
	{
		private readonly SqlConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlEventRepository"/> class.
		/// </summary>
		public SqlEventRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlEventRepository"/> class.
		/// Create SqlConnection to database for manipulation.
		/// </summary>
		/// <param name="connectionString">Connection string to database.</param>
		public SqlEventRepository(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
		}

		/// <summary>
		/// Getting a collection events from SQL database.
		/// </summary>
		/// <returns>Collection events.</returns>
		public async Task<IQueryable<Event>> GetAllAsync(int parentId)
		{
			var events = new List<Event>();

			var command = new SqlCommand(
				"SELECT DISTINCT Event.Id, Event.Name, Event.Description, LayoutId, DateTimeStart, DateTimeEnd, URL, VenueId " +
				"FROM Event, Layout, Venue " +
				"WHERE Event.LayoutId = Layout.Id AND Layout.VenueId = @id",
				_connection);
			command.Parameters.AddWithValue("@id", parentId);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				events.Add(new Event
				{
					Id = Convert.ToInt32(reader["Id"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
					LayoutId = Convert.ToInt32(reader["LayoutId"]),
					DateTimeStart = Convert.ToDateTime(reader["DateTimeStart"]),
					DateTimeEnd = Convert.ToDateTime(reader["DateTimeEnd"]),
					VenueId = Convert.ToInt32(reader["VenueId"]),
					URL = reader["URL"].ToString(),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return events.AsQueryable();
		}

		/// <summary>
		/// Getting collection events, satisfying the condition.
		/// </summary>
		/// <param name="start">Date-time start event.</param>
		/// <param name="end">Date-time end event.</param>
		/// <param name="partName">Part event name.</param>
		/// <returns>Collection events.</returns>
		public async Task<IQueryable<Event>> GetAllAsync(DateTime start, DateTime end, string partName)
		{
			var events = new List<Event>();

			var command = new SqlCommand(
				"SELECT DISTINCT Event.Id, Event.Name, Event.Description, LayoutId, DateTimeStart, DateTimeEnd, URL, VenueId " +
				"FROM Event, Layout, Venue " +
				"WHERE Event.LayoutId = Layout.Id AND Layout.VenueId = Venue.Id " +
				"AND Event.DateTimeStart >= @start AND Event.DateTimeEnd <= @end",
				_connection);

			if (partName != string.Empty && partName != null)
			{
				command.CommandText += " AND Event.Name LIKE CONCAT(@partName, '%')";
				command.Parameters.AddWithValue("@partName", partName);
			}

			command.Parameters.AddWithValue("@start", start);
			command.Parameters.AddWithValue("@end", end);

			// command.Parameters.AddWithValue("@partName", partName);
			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				events.Add(new Event
				{
					Id = Convert.ToInt32(reader["Id"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
					LayoutId = Convert.ToInt32(reader["LayoutId"]),
					DateTimeStart = Convert.ToDateTime(reader["DateTimeStart"]),
					DateTimeEnd = Convert.ToDateTime(reader["DateTimeEnd"]),
					VenueId = Convert.ToInt32(reader["VenueId"]),
					URL = reader["URL"].ToString(),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return events.AsQueryable();
		}

		/// <summary>
		/// Getting collection latest events.
		/// </summary>
		/// <param name="count">Count latest event.</param>
		/// <returns>Collection events.</returns>
		public async Task<IQueryable<Event>> GetLatestAsync(int count)
		{
			var events = new List<Event>();

			var command = new SqlCommand(
				"SELECT DISTINCT TOP(@count) Event.Id, Event.Name, Event.Description, LayoutId, DateTimeStart, DateTimeEnd, URL, VenueId " +
				"FROM Event, Layout, Venue " +
				"WHERE Event.LayoutId = Layout.Id AND Layout.VenueId = Venue.Id",
				_connection);
			command.Parameters.AddWithValue("@count", count);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				events.Add(new Event
				{
					Id = Convert.ToInt32(reader["Id"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
					LayoutId = Convert.ToInt32(reader["LayoutId"]),
					DateTimeStart = Convert.ToDateTime(reader["DateTimeStart"]),
					DateTimeEnd = Convert.ToDateTime(reader["DateTimeEnd"]),
					VenueId = Convert.ToInt32(reader["VenueId"]),
					URL = reader["URL"].ToString(),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return events.AsQueryable();
		}

		/// <summary>
		/// Get one event by id.
		/// </summary>
		/// <param name="id">Id event.</param>
		/// <returns>Event.</returns>
		public async Task<Event> GetAsync(int id)
		{
			Event userEvent = null;

			var command = new SqlCommand(
				"SELECT DISTINCT Event.Id, Event.Name, Event.Description, LayoutId, DateTimeStart, DateTimeEnd, URL, VenueId " +
				"FROM Event, Layout, Venue " +
				"WHERE Event.LayoutId = Layout.Id AND Layout.VenueId = Venue.Id AND Event.Id = @id",
				_connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				userEvent = new Event
				{
					Id = Convert.ToInt32(reader["Id"]),
					Name = reader["Name"].ToString(),
					Description = reader["Description"].ToString(),
					LayoutId = Convert.ToInt32(reader["LayoutId"]),
					DateTimeStart = Convert.ToDateTime(reader["DateTimeStart"]),
					DateTimeEnd = Convert.ToDateTime(reader["DateTimeEnd"]),
					VenueId = Convert.ToInt32(reader["VenueId"]),
					URL = reader["URL"].ToString(),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return userEvent;
		}

		/// <summary>
		/// Adding event to SQL database.
		/// </summary>
		/// <param name="entity">Event to add.</param>
		public async Task<bool> AddAsync(Event entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not add null object!");
			}

			var command = new SqlCommand("InsertEvent", _connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;

			command.Parameters.AddWithValue("@name", entity.Name);
			command.Parameters.AddWithValue("@description", entity.Description);
			command.Parameters.AddWithValue("@layoutId", entity.LayoutId);
			command.Parameters.AddWithValue("@dateTimeStart", entity.DateTimeStart);
			command.Parameters.AddWithValue("@dateTimeEnd", entity.DateTimeEnd);
			command.Parameters.AddWithValue("@areaPrice", entity.Price);
			command.Parameters.AddWithValue("@seatState", (int)SeatState.Free);
			command.Parameters.AddWithValue("@url", entity.URL);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Delete a event from a SQL database.
		/// </summary>
		/// <param name="id">Id event to delete.</param>
		public async Task<bool> DeleteAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Event id cannot be 0 or less.");
			}

			var command = new SqlCommand("DeleteEvent", _connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;

			command.Parameters.AddWithValue("@id", id);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Update event in SQL database.
		/// </summary>
		/// <param name="entity">Event to update.</param>
		public async Task<bool> UpdateAsync(Event entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not update null object!");
			}

			var command = new SqlCommand("UpdateEvent", _connection);
			command.CommandType = System.Data.CommandType.StoredProcedure;

			command.Parameters.AddWithValue("@id", entity.Id);
			command.Parameters.AddWithValue("@name", entity.Name);
			command.Parameters.AddWithValue("@description", entity.Description);
			command.Parameters.AddWithValue("@layoutId", entity.LayoutId);
			command.Parameters.AddWithValue("@dateTimeStart", entity.DateTimeStart);
			command.Parameters.AddWithValue("@dateTimeEnd", entity.DateTimeEnd);
			command.Parameters.AddWithValue("@areaPrice", entity.Price);
			command.Parameters.AddWithValue("@seatState", (int)entity.State);
			command.Parameters.AddWithValue("@url", entity.URL);

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
