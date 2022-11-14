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
    /// Testing the correctness of passed objects Venue to business logic methods.
    /// </summary>
    public class VenueValidationTest
	{
        private const string MESSAGE_EXCEPTION_DESCRIPTION = "Invalid Description: Description does not empty and longer then 120 characters.";
        private const string MESSAGE_EXCEPTION_ADD_NULL = "Can not add null venue. (Parameter 'venue')";
        private const string MESSAGE_EXCEPTION_ID = "Invalid Id: Id cannot be less 1.";
        private const string MESSAGE_EXCEPTION_VENUE_ID = "Venue id cannot be 0 or less.";
        private const string MESSAGE_EXCEPTION_NAME = "Invalid Name: Name does not empty and longer then 120 characters.";
        private const string MESSAGE_EXCEPTION_ADDRESS = "Invalid Address: Address does not empty and longer then 200 characters.";
        private const string MESSAGE_EXCEPTION_PHONE = "Invalid Phone: Phone does not longer then 30 characters.";
        private const string MESSAGE_EXCEPTION_UPDATE_NULL = "Can not update null venue. (Parameter 'venue')";

        [Test]
        public async Task VenueGet_WhenHaveCollection_ShouldReturnVenue()
        {
            // Arrange
            var listVenue = new List<Venue>
            {
                new Venue { Id = 3, Description = "Normal", Name = "Test venue", Address = "Homel" },
                new Venue { Id = 4, Description = "Very well", Name = "Test venue", Address = "Homel" },
                new Venue { Id = 6, Description = "Bad", Name = "Test venue", Address = "Homel" },
            };
            int id = 3;
            var venueServiceMock = new Mock<IServiceBase<Venue>>();
            venueServiceMock.Setup(m => m.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(listVenue.FirstOrDefault(x => x.Id == id)));

            var venueService = venueServiceMock.Object;

            // Act
            var actual = await venueService.GetAsync(id);

            // Assert
            Assert.AreEqual(listVenue[0], actual);
        }

        [Test]
        public void Add_WhenVenueIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => VenueValidationProvider.IsValidToAdd(null), MESSAGE_EXCEPTION_ADD_NULL);
        }

        [Test]
        public void AddUpdate_WhenVenueDescriptionFailed_ReturnException()
        {
            // Arrange
            var venue = new Venue { Id = 1, Name = "TestVenue", Address = "Homel", Description = null };
            
            // Act, Assert
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToAdd(venue), MESSAGE_EXCEPTION_DESCRIPTION);
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToUpdate(venue), MESSAGE_EXCEPTION_DESCRIPTION);
        }

        [Test]
        public void AddUpdate_WhenVenueNameFailed_ReturnException()
        {
            // Arrange
            var venue = new Venue { Id = 2, Phone = "123", Name = " ", Address = "Homel", Description = "Error name" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToAdd(venue), MESSAGE_EXCEPTION_NAME);
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToUpdate(venue), MESSAGE_EXCEPTION_NAME);
        }

        [Test]
        public void AddUpdate_WhenVenueAddressFailed_ReturnException()
        {
            // Arrange
            var venue = new Venue { Id = 1, Name = "TestVenue", Description = "Null address" };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToAdd(venue), MESSAGE_EXCEPTION_ADDRESS);
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToUpdate(venue), MESSAGE_EXCEPTION_ADDRESS);
        }

        [Test]
        public void AddUpdate_WhenVenuePhoneFailed_ReturnException()
        {
            // Arrange
            var venue = new Venue
            {
                Id = 1,
                Name = "TestVenue",
                Description = "Long Phone",
                Address = "Homel",
                Phone = "3583248987324; 843589273498; 348538957298;",
            };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToAdd(venue), MESSAGE_EXCEPTION_PHONE);
            Assert.Throws<ArgumentException>(() => VenueValidationProvider.IsValidToUpdate(venue), MESSAGE_EXCEPTION_PHONE);
        }

        [Test]
        public async Task Add_WhenVenueValidationProviderly_AddAndReturnTrue()
        {
            // Arrange
            var venueList = new List<Venue>();
            var venueServiceMock = new Mock<IServiceBase<Venue>>();
            venueServiceMock.Setup(m => m.AddAsync(It.IsAny<Venue>()))
                .Callback<Venue>(venue => venueList.Add(venue))
                .Returns(Task.FromResult(true));

            var venue = new Venue { Name = "Test Venue", Address = "Homel", Description = "Very Well" };

            // Act
            bool result = await venueServiceMock.Object.AddAsync(venue);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(venueList, new List<Venue> { venue });
        }

        [Test]
        public void Update_WnenVenueIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => VenueValidationProvider.IsValidToUpdate(null), MESSAGE_EXCEPTION_UPDATE_NULL);
        }

        [Test]
        public void Update_WhenVenueIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => 
                VenueValidationProvider.IsValidToUpdate(new Venue { Id = -2 }), MESSAGE_EXCEPTION_ID);
        }

        [Test]
        public async Task Update_WhenVenueValidationProviderly_UpdateAndReturnTrue()
        {
            // Arrange
            var venue = new Venue { Id = 1, Name = "Test Venu", Address = "Homel", Description = "Desc" };
            var venueList = new List<Venue> { venue };

            var venueServiceMock = new Mock<IServiceBase<Venue>>();
            venueServiceMock.Setup(m => m.UpdateAsync(It.IsAny<Venue>())).Callback<Venue>(venue =>
            {
                venueList.RemoveAt(0);
                venueList.Add(venue);
            }).Returns(Task.FromResult(true));

            // Act
            venue.Name = "New name Venue";
            bool result = await venueServiceMock.Object.UpdateAsync(venue);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(venueList, new List<Venue> { venue });
        }

        [Test]
        public void Delete_WhenVenueIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                VenueValidationProvider.IsValidToDelete(-1), MESSAGE_EXCEPTION_VENUE_ID);
        }

        [Test]
        public async Task Delete_WnenVenueValidationProviderly_DeleteAndReturnTrue()
        {
            // Arrange
            var venue = new Venue { Id = 0, Address = "Homel", Name = "Test Venue", Description = "Hey" };
            var venueList = new List<Venue> { venue };

            var venueServiceMock = new Mock<IServiceBase<Venue>>();
            venueServiceMock.Setup(m => m.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(venue => venueList.RemoveAt(venue))
                .Returns(Task.FromResult(true));

            // Act
            bool result = await venueServiceMock.Object.DeleteAsync(venue.Id);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.IsEmpty(venueList);
        }
    }
}
