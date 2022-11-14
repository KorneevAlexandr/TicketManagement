using System;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.UnitTests.TestsDALNullException
{
    /// <summary>
    /// Testing VenueProvider for exceptions when passing null.
    /// </summary>
    public class VenueProviderTest
	{
        [Test]
        public void VenueAdd_WhenVenueIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlVenueRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddAsync(null), "Can not add null object! (Parameter 'venue')");
        }

        [Test]
        public void VenueDelete_WhenVenueIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlVenueRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteAsync(0), "Venue id cannot be 0 or less.");
        }

        [Test]
        public void VenueUpdate_WhenVenueIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlVenueRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateAsync(null), "Can not update null object! (Parameter 'venue')");
        }
    }
}
