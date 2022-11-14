using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Repositories;
using TicketManagement.IntegrationTests.Data;

namespace TicketManagement.IntegrationTests.TestDALProviders
{
	/// <summary>
	/// Testing eventAreaRepo use test database.
	/// </summary>
	public class EventAreaRepositoryTest
	{
		private string _connectionString;

		/// <summary>
		/// Retrieving and Initializing the test database connection string.
		/// </summary>
		[SetUp]
		public void Setup()
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appSettings.json").Build();

			_connectionString = config.GetConnectionString("TestConnectionString");
		}

		[Test]
		public async Task DeleteEventArea_WhenEventAreaExist_ReturnEmptyCollection()
		{
			// Arrange
			var userEvent = new Event
			{
				LayoutId = LayoutData.GetLayouts[0].Id,
				Name = "Unuq Event Name",
				Description = "Good Event",
				DateTimeStart = new DateTime(2012, 12, 10),
				DateTimeEnd = new DateTime(2012, 12, 20),
				VenueId = 1,
				URL = "/",
			};
			var eventRepo = new SqlEventRepository(_connectionString);
			var eventAreaRepo = new SqlEventAreaRepository(_connectionString);

			// Act
			await eventRepo.AddAsync(userEvent);
			var eventLastId_s = await eventRepo.GetLatestAsync(1);
			var eventLastId	= eventLastId_s.Last().Id;
			var startEventAreas = await eventAreaRepo.GetPlacesAsync(eventLastId);

			await eventRepo.DeleteAsync(eventLastId);
			var endEventAreas = await eventAreaRepo.GetPlacesAsync(eventLastId);
			
			// Assert
			Assert.IsNotEmpty(startEventAreas);
			Assert.IsEmpty(endEventAreas);
		}

		[Test]
		public async Task UpdateEventArea_WhenEventAreaExist_ReturnNewEventArea()
		{
			// Arrange
			var userEvent = new Event
			{
				LayoutId = LayoutData.GetLayouts[0].Id,
				Name = "Unuq Event Name",
				Description = "Good Event",
				DateTimeStart = new DateTime(2012, 12, 10),
				DateTimeEnd = new DateTime(2012, 12, 20),
				VenueId = 1,
				URL = "/",
			};
			var eventRepo = new SqlEventRepository(_connectionString);
			var eventAreaRepo = new SqlEventAreaRepository(_connectionString);

			// Act
			await eventRepo.AddAsync(userEvent);
			var eventLastId_s = await eventRepo.GetLatestAsync(1);
			var eventLastId = eventLastId_s.Last().Id;
			var allLastEventArea = await eventAreaRepo.GetPlacesAsync(eventLastId);
			var lastEventArea = allLastEventArea.Last();

			lastEventArea.CoordX = 911;
			await eventAreaRepo.UpdateAsync(lastEventArea);

			var lastUpEventAreas = await eventAreaRepo.GetPlacesAsync(eventLastId);
			await eventRepo.DeleteAsync(eventLastId);

			// Assert
			Assert.AreEqual(lastEventArea.CoordX, lastUpEventAreas.Last().CoordX);
		}
	}
}
