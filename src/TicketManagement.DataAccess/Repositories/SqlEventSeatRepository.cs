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
	/// Represents access to the SQL database for event-seats getting.
	/// </summary>
	internal class SqlEventSeatRepository : IEventPlaceRepository<EventSeat>, IDisposable, IAsyncDisposable
	{
		private readonly SqlConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlEventSeatRepository"/> class.
		/// </summary>
		public SqlEventSeatRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlEventSeatRepository"/> class.
		/// Create SqlConnection to database for manipulation.
		/// </summary>
		/// <param name="connectionString">Connection string to database.</param>
		public SqlEventSeatRepository(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
		}


		/// <summary>
		/// Getting a collection seats from SQL database, owned by event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection event-seats.</returns>
		public async Task<IQueryable<EventSeat>> GetPlacesAsync(int id)
		{
			var eventSeats = new List<EventSeat>();

			var command = new SqlCommand(
				"SELECT * FROM EventSeat " +
				"WHERE EventSeat.EventAreaId = @id", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				eventSeats.Add(new EventSeat
				{
					Id = Convert.ToInt32(reader["Id"]),
					EventAreaId = Convert.ToInt32(reader["EventAreaId"]),
					Row = Convert.ToInt32(reader["Row"]),
					Number = Convert.ToInt32(reader["Number"]),
					State = (SeatState)Convert.ToInt32(reader["State"]),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return eventSeats.AsQueryable();
		}

		/// <summary>
		/// Getting a collection places from a data set, owned be event.
		/// </summary>
		/// <param name="id">Any event.</param>
		/// <returns>Collection seats or areas, owned by event.</returns>
		public async Task<IQueryable<EventSeat>> GetPlacesByEventIdAsync(int id)
		{
			var eventSeats = new List<EventSeat>();

			var command = new SqlCommand(
				"SELECT * FROM EventSeat, EventArea " +
				"WHERE EventAreaId = EventArea.Id AND EventArea.EventId = @id", 
				_connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				eventSeats.Add(new EventSeat
				{
					Id = Convert.ToInt32(reader["Id"]),
					EventAreaId = Convert.ToInt32(reader["EventAreaId"]),
					Row = Convert.ToInt32(reader["Row"]),
					Number = Convert.ToInt32(reader["Number"]),
					State = (SeatState)Convert.ToInt32(reader["State"]),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return eventSeats.AsQueryable();
		}

		/// <summary>
		/// Get one eventSeat by id.
		/// </summary>
		/// <param name="id">Id eventSeat.</param>
		/// <returns>EventSeat.</returns>
		public async Task<EventSeat> GetAsync(int id)
		{
			EventSeat eventSeat = null;

			var command = new SqlCommand("SELECT * FROM EventSeat WHERE Id = @id", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				eventSeat = new EventSeat
				{
					Id = Convert.ToInt32(reader["Id"]),
					EventAreaId = Convert.ToInt32(reader["EventAreaId"]),
					Row = Convert.ToInt32(reader["Row"]),
					Number = Convert.ToInt32(reader["Number"]),
					State = (SeatState)Convert.ToInt32(reader["State"]),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return eventSeat;
		}

		/// <summary>
		/// Delete a event-seat from a SQL database.
		/// </summary>
		/// <param name="id">Id event-seat to delete.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("EventSeat id cannot be 0 or less.");
			}

			var command = new SqlCommand("DELETE FROM EventSeat WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", id);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Update event-seat in SQL database.
		/// </summary>
		/// <param name="entity">EventSeat to update.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> UpdateAsync(EventSeat entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not update null object!");
			}

			var command = new SqlCommand("UPDATE EventSeat SET " +
				"Row = (@row), Number = (@number), State = (@state)" +
				"WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", entity.Id);
			command.Parameters.AddWithValue("@row", entity.Row);
			command.Parameters.AddWithValue("@number", entity.Number);
			command.Parameters.AddWithValue("@state", (int)entity.State);
			
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
