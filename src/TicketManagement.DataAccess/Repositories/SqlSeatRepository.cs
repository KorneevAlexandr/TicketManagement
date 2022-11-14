using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.Repositories;
using System.Data.SqlClient;

namespace TicketManagement.DataAccess.Repositories
{
	/// <summary>
	/// Represents access to the SQL database for seats manipulation.
	/// </summary>
	internal class SqlSeatRepository : IBaseRepository<Seat>, IDisposable, IAsyncDisposable
	{
		private readonly SqlConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlSeatRepository"/> class.
		/// </summary>
		public SqlSeatRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlSeatRepository"/> class.
		/// Create SqlConnection to database for manipulation.
		/// </summary>
		/// <param name="connectionString">Connection string to database.</param>
		public SqlSeatRepository(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
		}

		/// <summary>
		/// Getting a collection seats from SQL database.
		/// </summary>
		/// <returns>Collection seats.</returns>
		public async Task<IQueryable<Seat>> GetAllAsync(int parentId)
		{
			var seats = new List<Seat>();

			var command = new SqlCommand("SELECT * FROM Seat WHERE AreaId = @id", _connection);
			command.Parameters.AddWithValue("@id", parentId);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				seats.Add(new Seat
				{
					Id = Convert.ToInt32(reader["Id"]),
					AreaId = Convert.ToInt32(reader["AreaId"]),
					Row = Convert.ToInt32(reader["Row"]),
					Number = Convert.ToInt32(reader["Number"]),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return seats.AsQueryable();
		}

		/// <summary>
		/// Get one seat by id.
		/// </summary>
		/// <param name="id">Id seat.</param>
		/// <returns>Seat.</returns>
		public async Task<Seat> GetAsync(int id)
		{
			Seat seat = null;

			var command = new SqlCommand("SELECT * FROM Seat WHERE Id = @id", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				seat = new Seat
				{
					Id = Convert.ToInt32(reader["Id"]),
					AreaId = Convert.ToInt32(reader["AreaId"]),
					Row = Convert.ToInt32(reader["Row"]),
					Number = Convert.ToInt32(reader["Number"]),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return seat;
		}

		/// <summary>
		/// Adding seat to SQL database.
		/// </summary>
		/// <param name="entity">Seat to add.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> AddAsync(Seat entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not add null object!");
			}

			var command = new SqlCommand("INSERT INTO Seat (AreaId, Row, Number) " + 
				"VALUES (@areaId, @row, @number)", _connection);

			command.Parameters.AddWithValue("@areaId", entity.AreaId);
			command.Parameters.AddWithValue("@row", entity.Row);
			command.Parameters.AddWithValue("@number", entity.Number);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Delete a seat from a SQL database.
		/// </summary>
		/// <param name="id">Id seat to delete.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Seat id cannot be 0 or less.");
			}

			var command = new SqlCommand("DELETE FROM Seat WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", id);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Update seat in SQL database.
		/// </summary>
		/// <param name="entity">Seat to update.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> UpdateAsync(Seat entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not update null object!");
			}

			var command = new SqlCommand("UPDATE Seat SET " +
				"AreaId = (@areaId), Row = (@row), Number = (@number) " +
				"WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", entity.Id);
			command.Parameters.AddWithValue("@areaId", entity.AreaId);
			command.Parameters.AddWithValue("@row", entity.Row);
			command.Parameters.AddWithValue("@number", entity.Number);

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
