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
	/// Represents access to the SQL database for areas manipulation.
	/// </summary>
	internal class SqlAreaRepository : IBaseRepository<Area>, IDisposable, IAsyncDisposable
	{
		private readonly SqlConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlAreaRepository"/> class.
		/// </summary>
		public SqlAreaRepository()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlAreaRepository"/> class.
		/// Create SqlConnection to database for manipulation.
		/// </summary>
		/// <param name="connectionString">Connection string to database.</param>
		public SqlAreaRepository(string connectionString)
		{
			_connection = new SqlConnection(connectionString);
			_connection.Open();
		}

		/// <summary>
		/// Getting a collection areas from SQL database.
		/// </summary>
		/// <returns>Collection areas.</returns>
		public async Task<IQueryable<Area>> GetAllAsync(int parentId)
		{
			var areas = new List<Area>();

			var command = new SqlCommand("SELECT * FROM Area WHERE LayoutId = @id", _connection);
			command.Parameters.AddWithValue("@id", parentId);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				areas.Add(new Area
				{
					Id = Convert.ToInt32(reader["Id"]),
					LayoutId = Convert.ToInt32(reader["LayoutId"]),
					Description = reader["Description"].ToString(),
					CoordX = Convert.ToInt32(reader["CoordX"]),
					CoordY = Convert.ToInt32(reader["CoordY"]),
				});
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return areas.AsQueryable();
		}

		/// <summary>
		/// Get one area by id.
		/// </summary>
		/// <param name="id">Id area.</param>
		/// <returns>Area.</returns>
		public async Task<Area> GetAsync(int id)
		{
			Area area = null;

			var command = new SqlCommand("SELECT * FROM Area WHERE Id = @id", _connection);
			command.Parameters.AddWithValue("@id", id);

			var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				area = new Area
				{
					Id = Convert.ToInt32(reader["Id"]),
					LayoutId = Convert.ToInt32(reader["LayoutId"]),
					Description = reader["Description"].ToString(),
					CoordX = Convert.ToInt32(reader["CoordX"]),
					CoordY = Convert.ToInt32(reader["CoordY"]),
				};
			}

			await reader.CloseAsync();
			await command.DisposeAsync();

			return area;
		}

		/// <summary>
		/// Adding area to SQL database.
		/// </summary>
		/// <param name="entity">Area to add.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> AddAsync(Area entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not add null object!");
			}

			var command = new SqlCommand("INSERT INTO Area (LayoutId, Description, CoordX, CoordY) " +
				"VALUES (@layoutId, @desc, @coordX, @coordY)", _connection);

			command.Parameters.AddWithValue("@layoutId", entity.LayoutId);
			command.Parameters.AddWithValue("@desc", entity.Description);
			command.Parameters.AddWithValue("@coordX", entity.CoordX);
			command.Parameters.AddWithValue("@coordY", entity.CoordY);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Delete a area from a SQL database.
		/// </summary>
		/// <param name="id">Id area to delete.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Area id cannot be 0 or less.");
			}

			var command = new SqlCommand("DELETE FROM Area WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", id);

			int changeRow = await command.ExecuteNonQueryAsync();
			await command.DisposeAsync();
			return changeRow > 0;
		}

		/// <summary>
		/// Update area in SQL database.
		/// </summary>
		/// <param name="entity">Area to update.</param>
		/// <returns>True - if successfull, else - false.</returns>
		public async Task<bool> UpdateAsync(Area entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Can not update null object!");
			}

			var command = new SqlCommand("UPDATE Area SET " +
				"LayoutId = (@layoutId), Description = (@desc), CoordX = (@coordX), CoordY = (@coordY) " +
				"WHERE Id = (@id)", _connection);

			command.Parameters.AddWithValue("@id", entity.Id);
			command.Parameters.AddWithValue("@layoutId", entity.LayoutId);
			command.Parameters.AddWithValue("@desc", entity.Description);
			command.Parameters.AddWithValue("@coordX", entity.CoordX);
			command.Parameters.AddWithValue("@coordY", entity.CoordY);

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
