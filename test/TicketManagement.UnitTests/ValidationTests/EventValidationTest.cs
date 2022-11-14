using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ValidationProviders;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.UnitTests.TestsBLValidation
{
    /// <summary>
    /// Testing the correctness of passed objects Event to business logic methods.
    /// </summary>
    public class EventValidationTest
	{
        private const string MESSAGE_EXCEPTION_DESCRIPTION = "Invalid Description: Description does not empty.";
        private const string MESSAGE_EXCEPTION_LAYOUT_ID = "Invalid LayoutId: such layout does not exist.";
        private const string MESSAGE_EXCEPTION_ADD_NULL = "Can not add null event. (Parameter 'userEvent')";
        private const string MESSAGE_EXCEPTION_ID = "Invalid Id: Id cannot be less 1.";
        private const string MESSAGE_EXCEPTION_EVENT_ID = "Event id cannot be 0 or less.";
        private const string MESSAGE_EXCEPTION_NAME = "Invalid Name: Name does not empty and longer then 120 characters.";
        private const string MESSAGE_EXCEPTION_PRICE = "Invalid Price: Price does not negative.";
        private const string MESSAGE_EXCEPTION_PAST_DATETIME = "Invalid DateTime: date and time event does not created in past.";
        private const string MESSAGE_EXCEPTION_BOOKED_DATETIME = "Invalid DateTime: the end of an event cannot be earlier than its start.";
        private const string MESSAGE_EXCEPTION_UPDATE_NULL = "Can not update null event. (Parameter 'userEvent')";

        [Test]
        public async Task EventGetAll_WhenHaveCollection_ShouldReturnEvent()
        {
            // Arrange
            var listEvent = new List<Event>
            {
                new Event { Id = 3, Description = "Normal", Name = "Test event", LayoutId = 4, VenueId = 1 },
                new Event { Id = 4, Description = "Very well", Name = "Test event", LayoutId = 3, VenueId = 1 },
                new Event { Id = 6, Description = "Bad", Name = "Test event", LayoutId = 2, VenueId = 1 },
            };
            var eventServiceMock = new Mock<IServiceBase<Event>>();
            eventServiceMock.Setup(m => m.GetAllAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(listEvent.Where(x => x.VenueId == 1)));

            var eventService = eventServiceMock.Object;

            // Act
            var actualList = await eventService.GetAllAsync(1);

            // Assert
            CollectionAssert.AreEquivalent(listEvent, actualList);
        }

        [Test]
        public void Add_WhenEventIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
                EventValidationProvider.IsValidToAdd(null), MESSAGE_EXCEPTION_ADD_NULL);
        }

        [Test]
        public void AddUpdate_WhenEventDescriptionFailed_ReturnException()
        {
            // Arrange
            var eventUser = new Event { Id = 1, Name = "TestEvent", LayoutId = 2 };
            
            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToAdd(eventUser), MESSAGE_EXCEPTION_DESCRIPTION);
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToUpdate(eventUser), MESSAGE_EXCEPTION_DESCRIPTION);
        }

        [Test]
        public void AddUpdate_WhenEventNameFailed_ReturnException()
        {
            // Arrange
            var eventUser = new Event { Id = 2, LayoutId = 1, Name = " ", Description = "Error name" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToAdd(eventUser), MESSAGE_EXCEPTION_NAME);
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToUpdate(eventUser), MESSAGE_EXCEPTION_NAME);
        }

        [Test]
        public void AddUpdate_WhenEventLayoutIdFailed_ReturnException()
        {
            // Arrange
            var eventUser = new Event
            {
                Id = 2, 
                LayoutId = -21, 
                Name = "Name Event", 
                Description = "Error name",
                DateTimeStart = DateTime.UtcNow.AddDays(1), 
                DateTimeEnd = DateTime.UtcNow.AddDays(2)
            };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToAdd(eventUser), MESSAGE_EXCEPTION_LAYOUT_ID);
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToUpdate(eventUser), MESSAGE_EXCEPTION_LAYOUT_ID);
        }

        [Test]
        public void AddUpdate_WhenEventPriceFailed_ReturnException()
        {
            // Arrange
            var eventUser = new Event
            {
                Id = 2,
                LayoutId = 2,
                Name = "Name Event",
                Description = "Error name",
                DateTimeStart = DateTime.UtcNow.AddDays(1),
                DateTimeEnd = DateTime.UtcNow.AddDays(2),
                Price = -2,
            };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToAdd(eventUser), MESSAGE_EXCEPTION_PRICE);
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToUpdate(eventUser), MESSAGE_EXCEPTION_PRICE);
        }

        [Test]
        public void AddUpdate_WhenEventDateTimeInPast_ReturnException()
        {
            // Arrange
            var eventUser = new Event
            {
                Id = 2,
                LayoutId = 2,
                Name = "Name Event",
                Description = "Error name",
                DateTimeStart = DateTime.UtcNow.AddDays(-2),
                DateTimeEnd = DateTime.UtcNow.AddDays(2),
                State = SeatState.Booked,
            };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToAdd(eventUser), MESSAGE_EXCEPTION_PAST_DATETIME);
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToUpdate(eventUser), MESSAGE_EXCEPTION_PAST_DATETIME);
        }

        [Test]
        public void AddUpdate_WhenEventDateTimeRepiodFailed_ReturnException()
        {
            // Arrange
            var eventUser = new Event
            {
                Id = 2,
                LayoutId = 2,
                Name = "Name Event",
                Description = "Error name",
                DateTimeStart = DateTime.UtcNow.AddDays(10),
                DateTimeEnd = DateTime.UtcNow.AddDays(2),
            };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToAdd(eventUser), MESSAGE_EXCEPTION_BOOKED_DATETIME);
            Assert.Throws<ArgumentException>(() => EventValidationProvider.IsValidToUpdate(eventUser), MESSAGE_EXCEPTION_BOOKED_DATETIME);
        }

        [Test]
        public async Task Add_WhenEventValidationProviderly_AddAndReturnTrue()
        {
            // Arrange
            var eventList = new List<Event>();
            var eventServiceMock = new Mock<IServiceBase<Event>>();
            eventServiceMock.Setup(m => m.AddAsync(It.IsAny<Event>()))
                .Callback<Event>(eventUser => eventList.Add(eventUser))
                .Returns(Task.FromResult(true));

            var eventUser = new Event
            {
                Id = 2,
                LayoutId = 2,
                Name = "Name Event",
                Description = "Error name",
                DateTimeStart = DateTime.UtcNow.AddDays(1),
                DateTimeEnd = DateTime.UtcNow.AddDays(2),
            };

            // Act
            bool result = await eventServiceMock.Object.AddAsync(eventUser);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(eventList, new List<Event> { eventUser });
        }

        [Test]
        public void Update_WnenEventIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => 
                EventValidationProvider.IsValidToUpdate(null), MESSAGE_EXCEPTION_UPDATE_NULL);
        }

        [Test]
        public void Update_WhenEventIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                EventValidationProvider.IsValidToUpdate(new Event { Id = -4 }), MESSAGE_EXCEPTION_ID);
        }

        [Test]
        public async Task Update_WhenEventValidationProviderly_UpdateAndReturnTrue()
        {
            // Arrange
            var eventUser = new Event
            {
                Id = 2,
                LayoutId = 2,
                Name = "Name Event",
                Description = "Error name",
                DateTimeStart = DateTime.UtcNow.AddDays(1),
                DateTimeEnd = DateTime.UtcNow.AddDays(2),
            };
            var eventList = new List<Event> { eventUser };

            var eventServiceMock = new Mock<IServiceBase<Event>>();
            eventServiceMock.Setup(m => m.UpdateAsync(It.IsAny<Event>())).Callback<Event>(eventUser =>
            {
                eventList.RemoveAt(0);
                eventList.Add(eventUser);
            }).Returns(Task.FromResult(true));

            // Act
            eventUser.Name = "New name Event";
            bool result = await eventServiceMock.Object.UpdateAsync(eventUser);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(eventList, new List<Event> { eventUser });
        }

        [Test]
        public void Delete_WhenEventIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                EventValidationProvider.IsValidToDelete(0), MESSAGE_EXCEPTION_EVENT_ID);
        }

        [Test]
        public async Task Delete_WnenEventValidationProviderly_DeleteAndReturnTrue()
        {
            // Arrange
            var eventUser = new Event
            {
                Id = 0,
                LayoutId = 2,
                Name = "Name Event",
                Description = "Error name",
                DateTimeStart = DateTime.UtcNow.AddDays(1),
                DateTimeEnd = DateTime.UtcNow.AddDays(2),
            };
            var eventList = new List<Event> { eventUser };

            var eventServiceMock = new Mock<IServiceBase<Event>>();
            eventServiceMock.Setup(m => m.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(eventUser => eventList.RemoveAt(eventUser))
                .Returns(Task.FromResult(true));

            // Act
            bool result = await eventServiceMock.Object.DeleteAsync(eventUser.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.IsEmpty(eventList);
        }

        [Test]
        public void GetAllEventAreas_WnenEventIdFailed_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                EventValidationProvider.IsValidEventToPlaces(-1), MESSAGE_EXCEPTION_ID);
        }
    }
}
