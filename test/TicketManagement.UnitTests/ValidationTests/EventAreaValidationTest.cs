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
    /// Testing the correctness of passed objects EventArea to business logic methods.
    /// </summary>
    public class EventAreaValidationTest
	{
        private const string MESSAGE_EXCEPTION_DESCRIPTION = "Invalid Description: Description does not empty and longer then 200 characters.";
        private const string MESSAGE_EXCEPTION_COORD_X = "Invalid CoordX: CoordX cannot be less 1.";
        private const string MESSAGE_EXCEPTION_COORD_Y = "Invalid CoordY: CoordY cannot be less 1.";
        private const string MESSAGE_EXCEPTION_LAYOUT_ID = "Invalid LayoutId: such layout does not exist.";
        private const string MESSAGE_EXCEPTION_AREA_ID = "Area id cannot be 0 or less.";
        private const string MESSAGE_EXCEPTION_ID = "Invalid Id: Id cannot be less 1.";
        private const string MESSAGE_EXCEPTION_UPDATE_NULL = "Can not update null area. (Parameter 'area')";

        [Test]
        public async Task GetAllEventAreas_WhenHaveCollection_ShouldReturnEventAreas()
        {
            // Arrange
            var listEventAreas = new List<EventArea>
            {
                new EventArea { Id = 3, Description = "Normal1", EventId = 2 },
                new EventArea { Id = 4, Description = "Normal2", EventId = 3 },
                new EventArea { Id = 6, Description = "Normal3", EventId = 2 },
                new EventArea { Id = 7, Description = "Normal4", EventId = 3 },
                new EventArea { Id = 8, Description = "Normal5", EventId = 5 },
            };
            var eventUser = new Event { Id = 5 };
            var eventServiceMock = new Mock<IEventPlaceService<EventArea>>();
            eventServiceMock.Setup(m => m.GetPlacesAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(listEventAreas.Where(x => x.EventId == eventUser.Id)));

            var eventService = eventServiceMock.Object;

            // Act
            var actualList = await eventService.GetPlacesAsync(eventUser.Id);

            // Assert
            Assert.AreEqual(listEventAreas.Last().Id, actualList.ToList()[0].Id);
        }

        [Test]
        public void Update_WhenEventAreaLayoutIdFailed_ReturnException()
        {
            // Arrange
            var area = new EventArea { Id = 1, LayoutId = -4, CoordX = 1, CoordY = 2, Description = "Well" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventAreaValidationProvider.IsValidToUpdate(area), MESSAGE_EXCEPTION_LAYOUT_ID);
        }

        [Test]
        public void Update_WhenEventAreaDescriptionFailed_ReturnException()
        {
            // Arrange
            var area = new EventArea { Id = 1, LayoutId = 1, CoordX = 1, CoordY = 2, Description = "  " };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventAreaValidationProvider.IsValidToUpdate(area), MESSAGE_EXCEPTION_DESCRIPTION);
        }

        [Test]
        public void Update_WhenEventAreaCoordsFailed_ReturnException()
        {
            // Arrange
            var areaX = new EventArea { Id = 1, LayoutId = 1, CoordX = -1, CoordY = 2, Description = "Error X" };
            var areaY = new EventArea { Id = 1, LayoutId = 1, CoordX = 1, CoordY = -2, Description = "Error Y" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventAreaValidationProvider.IsValidToUpdate(areaX), MESSAGE_EXCEPTION_COORD_X);
            Assert.Throws<ArgumentException>(() => EventAreaValidationProvider.IsValidToUpdate(areaY), MESSAGE_EXCEPTION_COORD_Y);
        }

        [Test]
        public void Update_WnenAreaIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => EventAreaValidationProvider.IsValidToUpdate(null), MESSAGE_EXCEPTION_UPDATE_NULL);
        }

        [Test]
        public void Update_WhenEventAreaIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                EventAreaValidationProvider.IsValidToUpdate(new EventArea { Id = -3 }), MESSAGE_EXCEPTION_ID);
        }

        [Test]
        public void Delete_WhenEventAreaIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => EventAreaValidationProvider.IsValidToDelete(-3), MESSAGE_EXCEPTION_AREA_ID);
        }
    }
}
