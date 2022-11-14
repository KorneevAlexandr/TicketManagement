using System;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.UnitTests.TestsDALNullException
{
    /// <summary>
    /// Testing LayoutProvider for exceptions when passing null.
    /// </summary>
    public class LayoutProviderTest
	{
        [Test]
        public void LayoutAdd_WhenLayoutIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlLayoutRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddAsync(null), "Can not add null object! (Parameter 'layout')");
        }

        [Test]
        public void LayoutDelete_WhenLayoutIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlLayoutRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteAsync(0), "Layout id cannot be 0 or less.");
        }

        [Test]
        public void LayoutUpdate_WhenLayoutIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlLayoutRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateAsync(null), "Can not update null object! (Parameter 'layout')");
        }
    }
}
