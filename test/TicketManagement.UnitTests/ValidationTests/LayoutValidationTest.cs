using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ValidationProviders;
using Moq;

namespace TicketManagement.UnitTests.TestsBLValidation
{
    /// <summary>
    /// Testing the correctness of passed objects Layout to business logic methods.
    /// </summary>
    public class LayoutValidationTest
	{
        private const string MESSAGE_EXCEPTION_DESCRIPTION = "Invalid Description: Description does not empty and longer then 120 characters.";
        private const string MESSAGE_EXCEPTION_LAYOUT_ID = "Layout id cannot be 0 or less.";
        private const string MESSAGE_EXCEPTION_ADD_NULL = "Can not add null layout. (Parameter 'layout')";
        private const string MESSAGE_EXCEPTION_ID = "Invalid Id: Id cannot be less 1.";
        private const string MESSAGE_EXCEPTION_VENUE_ID = "Invalid VenueId: such venue does not exist.";
        private const string MESSAGE_EXCEPTION_NAME = "Invalid Name: Name does not empty and longer then 120 characters.";
        private const string MESSAGE_EXCEPTION_UPDATE_NULL = "Can not update null layout. (Parameter 'layout')";

        [Test]
        public async Task LayoutGetAll_WhenHaveCollection_ShouldReturnLayout()
        {
            // Arrange
            var listLayout = new List<Layout>
            {
                new Layout { Id = 3, Description = "Normal", VenueId = 2, Name = "name" },
                new Layout { Id = 4, Description = "Very well", VenueId = 3, Name = "name" },
                new Layout { Id = 6, Description = "Bad", VenueId = 4, Name = "name" },
            };
            int id = 3;
            var layoutServiceMock = new Mock<IServiceBase<Layout>>();
            layoutServiceMock.Setup(m => m.GetAllAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(listLayout.Where(x => x.VenueId == id)));

            var layoutService = layoutServiceMock.Object;

            // Act
            var actualList = await layoutService.GetAllAsync(id);

            // Assert
            CollectionAssert.AreEquivalent(listLayout.Where(x => x.VenueId == 3), actualList);
        }

        [Test]
        public void Add_WhenLayoutIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
                LayoutValidationProvider.IsValidToAdd(null), MESSAGE_EXCEPTION_ADD_NULL);
        }

        [Test]
        public void AddUpdate_WhenLayoutVenueIdFailed_ReturnException()
        {
            // Arrange
            var layout = new Layout { Id = 1, VenueId = 0, Name = "name", Description = "Well" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => LayoutValidationProvider.IsValidToAdd(layout), MESSAGE_EXCEPTION_VENUE_ID);
            Assert.Throws<ArgumentException>(() => LayoutValidationProvider.IsValidToUpdate(layout), MESSAGE_EXCEPTION_VENUE_ID);
        }

        [Test]
        public void AddUpdate_WhenLayoutDescriptionFailed_ReturnException()
        {
            // Arrange
            var layout = new Layout { Id = 1, VenueId = 1, Name = "name", Description = "  " };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => LayoutValidationProvider.IsValidToAdd(layout), MESSAGE_EXCEPTION_DESCRIPTION);
            Assert.Throws<ArgumentException>(() => LayoutValidationProvider.IsValidToUpdate(layout), MESSAGE_EXCEPTION_DESCRIPTION);
        }

        [Test]
        public void AddUpdate_WhenLayoutNameFailed_ReturnException()
        {
            // Arrange
            var layout = new Layout { Id = 1, VenueId = 1, Name = "  ", Description = "Error name" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => LayoutValidationProvider.IsValidToAdd(layout), MESSAGE_EXCEPTION_NAME);
            Assert.Throws<ArgumentException>(() => LayoutValidationProvider.IsValidToUpdate(layout), MESSAGE_EXCEPTION_NAME);
        }

        [Test]
        public async Task Add_WhenLayoutValidationProviderly_AddAndReturnTrue()
        {
            // Arrange
            var layoutList = new List<Layout>();
            var layoutServiceMock = new Mock<IServiceBase<Layout>>();
            layoutServiceMock.Setup(m => m.AddAsync(It.IsAny<Layout>()))
                .Callback<Layout>(layout => layoutList.Add(layout))
                .Returns(Task.FromResult(true));

            var layout = new Layout { VenueId = 1, Name = "name", Description = "Very Well" };

            // Act
            bool result = await layoutServiceMock.Object.AddAsync(layout);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(layoutList, new List<Layout> { layout });
        }

        [Test]
        public void Update_WnenLayoutIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => 
                LayoutValidationProvider.IsValidToUpdate(null), MESSAGE_EXCEPTION_UPDATE_NULL);
        }

        [Test]
        public void Update_WhenLayoutIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                LayoutValidationProvider.IsValidToUpdate(new Layout { Id = -2 }), MESSAGE_EXCEPTION_ID);
        }

        [Test]
        public async Task Update_WhenLayoutValidationProviderly_UpdateAndReturnTrue()
        {
            // Arrange
            var layout = new Layout { Id = 1, VenueId = 2, Name = "Simple name", Description = "Desc" };
            var layoutList = new List<Layout> { layout };

            var layoutServiceMock = new Mock<IServiceBase<Layout>>();
            layoutServiceMock.Setup(m => m.UpdateAsync(It.IsAny<Layout>()))
                .Callback<Layout>(layout =>
            {
                layoutList.RemoveAt(0);
                layoutList.Add(layout);
            }).Returns(Task.FromResult(true));

            // Act
            layout.Name = "New name";
            bool result = await layoutServiceMock.Object.UpdateAsync(layout);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(layoutList, new List<Layout> { layout });
        }

        [Test]
        public void Delete_WhenLayoutIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                LayoutValidationProvider.IsValidToDelete(0), MESSAGE_EXCEPTION_LAYOUT_ID);
        }

        [Test]
        public async Task Delete_WnenLayoutValidationProviderly_DeleteAndReturnTrue()
        {
            // Arrange
            var layout = new Layout { Id = 0, VenueId = 2, Name = "Simple name", Description = "Hey" };
            var layoutList = new List<Layout> { layout };

            var layoutServiceMock = new Mock<IServiceBase<Layout>>();
            layoutServiceMock.Setup(m => m.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(layout => layoutList.RemoveAt(layout))
                .Returns(Task.FromResult(true));

            // Act
            bool result = await layoutServiceMock.Object.DeleteAsync(layout.Id);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.IsEmpty(layoutList);
        }
    }
}
