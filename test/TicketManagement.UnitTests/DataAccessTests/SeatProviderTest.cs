using System;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.UnitTests.TestsDALNullException
{
    /// <summary>
    /// Testing SeatProvider for exceptions when passing null.
    /// </summary>
    public class SeatProviderTest
	{
        [Test]
        public void SeatAdd_WhenSeatIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlSeatRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddAsync(null), "Can not add null object! (Parameter 'seat')");
        }

        [Test]
        public void SeatDelete_WhenSeatIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlSeatRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteAsync(0), "Seat id cannot be 0 or less.");
        }

        [Test]
        public void SeatUpdate_WhenSeatIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlSeatRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateAsync(null), "Can not update null object! (Parameter 'seat')");
        }
    }
}
