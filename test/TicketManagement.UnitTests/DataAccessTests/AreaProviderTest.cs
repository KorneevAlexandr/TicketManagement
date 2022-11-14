using System;
using System.Threading.Tasks;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.UnitTests.TestsDALNullException
{
    /// <summary>
    /// Testing AreaProvider for exceptions when passing null.
    /// </summary>
	public class AreaProviderTest
	{
		[Test]
		public void AreaAdd_WhenAreaIsNull_ShouldThrowException()
		{
			// Arrange
			var repo = new SqlAreaRepository();

			// Act, Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.AddAsync(null), "Can not add null object! (Parameter 'area')");
		}

		[Test]
        public void AreaDelete_WhenAreaIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlAreaRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await repo.DeleteAsync(0), "Area id cannot be 0 or less.");
        }

        [Test]
        public void AreaUpdate_WhenAreaIsNull_ShouldThrowException()
        {
            // Arrange
            var repo = new SqlAreaRepository();

            // Act, Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await repo.UpdateAsync(null), "Can not update null object! (Parameter 'area')");
        }
    }
}
