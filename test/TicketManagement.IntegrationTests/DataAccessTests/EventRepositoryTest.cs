using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.IntegrationTests.Data;
using TicketManagement.DataAccess.Repositories;
using TicketManagement.DataAccess.Domain;
using FluentAssertions;

namespace TicketManagement.IntegrationTests.TestDALRepos
{
    /// <summary>
    /// Testing calls to DAL-methods, that call Stored Procedure for manipulation events.
    /// Testing SqleventRepo, SqlEventAreaRepo, SqlEventSeatRepo.
    /// </summary>
    public class EventRepositoryTest
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
        public async Task AddDeleteEvent_WhenAddNewEvent_ReturnEarlyAddedEvent()
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

            // Act
            await eventRepo.AddAsync(userEvent);
            var addedEvents = await eventRepo.GetAllAsync(userEvent.VenueId);
            var addedEvent = addedEvents.Last();
            await eventRepo.DeleteAsync(addedEvent.Id);

            // Assert
            userEvent.Should().BeEquivalentTo(addedEvent, options => options.Excluding(x => x.Id));
        }

        [Test]
        public async Task GetAllEvents_AddTwoEvents_ReturnSomeTwoEvents()
        {
            // Arrange
            var eventList = new List<Event>
            {
                new Event
                {
                    LayoutId = LayoutData.GetLayouts[0].Id,
                    Name = "Unuq Event Name",
                    Description = "Good Event",
                    DateTimeStart = new DateTime(2012, 12, 10),
                    DateTimeEnd = new DateTime(2012, 12, 20),
                    VenueId = 1,
                    URL = "/",
                },
                new Event
                {
                    LayoutId = LayoutData.GetLayouts[1].Id,
                    Name = "Unuq Event Name TWO",
                    Description = "Good Event TWO",
                    DateTimeStart = new DateTime(2011, 12, 10),
                    DateTimeEnd = new DateTime(2011, 12, 20),
                    VenueId = 1,
                    URL = "/",
                },
            };
            var eventRepo = new SqlEventRepository(_connectionString);

            // Act
            await eventRepo.AddAsync(eventList[0]);
            await eventRepo.AddAsync(eventList[1]);
            var newEventList = await eventRepo.GetAllAsync(1);
            await eventRepo.DeleteAsync(newEventList.Reverse().First().Id);
            await eventRepo.DeleteAsync(newEventList.Reverse().First().Id);

            // Assert
            eventList.Should().BeEquivalentTo(newEventList.TakeLast(eventList.Count), options => options.Excluding(x => x.Id));
        }

        [Test]
        public async Task AddEvent_WhenAutomaticAddSeatAndArea_ReturnSuitableSeatsAndAreas()
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
            var eventSeatRepo = new SqlEventSeatRepository(_connectionString);

            // Act
            await eventRepo.AddAsync(userEvent);
            var allEvents = await eventRepo.GetAllAsync(userEvent.VenueId);
            userEvent.Id = allEvents.Last().Id;
            var actualAreas = await eventAreaRepo.GetPlacesAsync(userEvent.Id);
            var actualSeats = await eventSeatRepo.GetPlacesAsync(userEvent.Id);
            await eventRepo.DeleteAsync(userEvent.Id);

            // Assert
            actualAreas.Should().NotBeEmpty();
            actualSeats.Should().BeEmpty(); 
        }

        [Test]
        public async Task DeleteEvent_WhenDeleteEventDeleteHimSeatAndArea_ReturnEmptyCollections()
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

            var startAreas = new List<EventArea>();

            // Act
            await eventRepo.AddAsync(userEvent);
            var allEvents = await eventRepo.GetAllAsync(userEvent.VenueId);
            userEvent.Id = allEvents.Last().Id;
            var middleAreas = await eventAreaRepo.GetPlacesAsync(userEvent.Id);
            await eventRepo.DeleteAsync(userEvent.Id);
            var endAreas = await eventAreaRepo.GetPlacesAsync(userEvent.Id);

            // Assert   
            startAreas.Should().NotBeEquivalentTo(middleAreas);
            startAreas.Should().BeEquivalentTo(endAreas);
            endAreas.Should().HaveCount(0);
        }

        [Test]
        public async Task UpdateEvent_WhenWithoutChangeLayoutID_ReturnSameEvent()
        {
            // Arrange
            var userEvent = new Event
            {
                LayoutId = LayoutData.GetLayouts[0].Id, // same
                Name = "Unuq Event Name",
                Description = "Good Event",
                DateTimeStart = new DateTime(2012, 12, 10),
                DateTimeEnd = new DateTime(2012, 12, 20),
                VenueId = 1,
                URL = "/",
            };
            var updateEvent = new Event
            {
                LayoutId = LayoutData.GetLayouts[0].Id, // same
                Name = "New Test Name!",
                Description = "Ok",
                DateTimeStart = new DateTime(2010, 12, 12),
                DateTimeEnd = new DateTime(2020, 12, 12),
                VenueId = 1,
                URL = "/",
            };
            var eventRepo = new SqlEventRepository(_connectionString);

            // Act
            await eventRepo.AddAsync(userEvent);
            var allEvents = await eventRepo.GetAllAsync(userEvent.VenueId);
            updateEvent.Id = allEvents.First().Id;
            await eventRepo.UpdateAsync(updateEvent);
            var newUpdateEvents = await eventRepo.GetAllAsync(userEvent.VenueId);
            var newUpdateEvent = newUpdateEvents.First();
            await eventRepo.DeleteAsync(newUpdateEvent.Id);

            // Assert
            updateEvent.Should().BeEquivalentTo(newUpdateEvent);
        }

        [Test]
        public async Task UpdateEvent_WhenChangeLayoutID_ReturnNewCollectionSeatsAndArea()
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

            // Act
            await eventRepo.AddAsync(userEvent);
            var allEvents = await eventRepo.GetAllAsync(userEvent.VenueId);
            userEvent.Id = allEvents.Last().Id;

            userEvent.LayoutId = LayoutData.GetLayouts.First(x => x.Id != userEvent.LayoutId).Id;
            await eventRepo.UpdateAsync(userEvent);

            var newUpdateEvents = await eventRepo.GetAllAsync(userEvent.VenueId);
            var newUpdateEvent = newUpdateEvents.Last();
            await eventRepo.DeleteAsync(userEvent.Id);

            // Assert
            userEvent.Should().BeEquivalentTo(newUpdateEvent);
        }
    }
}
