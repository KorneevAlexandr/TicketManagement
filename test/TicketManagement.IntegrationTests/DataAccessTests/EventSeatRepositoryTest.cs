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
	/// Testing eventSeatRepo use test database.
	/// </summary>
	public class EventSeatRepositoryTest
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
		public async Task DeleteEventSeat_WhenEventSeatxist_ReturnEmptyCollection()
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
			var eventSeatRepo = new SqlEventSeatRepository(_connectionString);

			// Act
			await eventRepo.AddAsync(userEvent);
			var eventLastId = await eventRepo.GetAllAsync(userEvent.VenueId);
			var startEventSeats = await eventSeatRepo.GetPlacesByEventIdAsync(eventLastId.Last().Id);

			foreach (var eventSeat in startEventSeats)
			{
				await eventSeatRepo.DeleteAsync(eventSeat.Id);
			}

			var endEventSeats = await eventSeatRepo.GetPlacesByEventIdAsync(eventLastId.Last().Id);
			await eventRepo.DeleteAsync(eventLastId.Last().Id);

			// Assert
			Assert.IsNotEmpty(startEventSeats);
			Assert.IsEmpty(endEventSeats);
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
			var eventSeatRepo = new SqlEventSeatRepository(_connectionString);

			// Act
			await eventRepo.AddAsync(userEvent);
			var eventLastId = await eventRepo.GetAllAsync(userEvent.VenueId);
			var lastEventSeat = await eventSeatRepo.GetPlacesByEventIdAsync(eventLastId.Last().Id);

			lastEventSeat.Last().Number = 911;
			await eventSeatRepo.UpdateAsync(lastEventSeat.Last());

			var lastUpEventSeat = await eventSeatRepo.GetPlacesByEventIdAsync(eventLastId.Last().Id);
			await eventRepo.DeleteAsync(eventLastId.Last().Id);

			// Assert
			Assert.AreEqual(lastEventSeat.Last().Number, lastUpEventSeat.Last().Number);
		}
	}
}
