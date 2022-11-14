using System;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.UnitTests.TestsDALNullException
{   
    /// <summary>
     /// Testing EventProvider for exceptions when passing null.
     /// </summary>
    public class EventProviderTest
	{
        [Test]
        public void EventAdd_WhenEventIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlEventRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddAsync(null), "Can not add null object! (Parameter 'userEvent')");
        }

        [Test]
        public void EventDelete_WhenEventIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlEventRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteAsync(0), "Event id cannot be 0 or less.");
        }

        [Test]
        public void EventUpdate_WhenEventIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlEventRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateAsync(null), "Can not update null object! (Parameter 'userEvent')");
        }
    }
}
