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
    /// Testing the correctness of passed objects Seat to business logic methods.
    /// </summary>
    public class SeatValidationTest
    {
        private const string MESSAGE_EXCEPTION_ADD_NULL = "Can not add null seat. (Parameter 'seat')";
        private const string MESSAGE_EXCEPTION_AREA_ID = "Invalid AreaId: such area does not exist.";
        private const string MESSAGE_EXCEPTION_ROW = "Invalid Row: Row cannot be less 1.";
        private const string MESSAGE_EXCEPTION_NUMBER = "Invalid Number: Number cannot be less 1.";
        private const string MESSAGE_EXCEPTION_ID = "Invalid Id: Id cannot be less 1.";
        private const string MESSAGE_EXCEPTION_SEAT_ID = "Seat id cannot be 0 or less.";
        private const string MESSAGE_EXCEPTION_UPDATE_NULL = "Can not update null seat. (Parameter 'seat')";

        [Test]
        public async Task SeatGetAll_WhenHaveCollection_ShouldReturnSeats()
        {
            // Arrange
            var listSeat = new List<Seat>
            {
                new Seat { Id = 3, AreaId = 2, Number = 1, Row = 2 },
                new Seat { Id = 4, AreaId = 2, Number = 1, Row = 3 },
                new Seat { Id = 6, AreaId = 3, Number = 1, Row = 4 },
            };
            var seatServiceMock = new Mock<IServiceBase<Seat>>();
            seatServiceMock.Setup(m => m.GetAllAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(listSeat.Where(s => s.AreaId == 2)));

            var seatService = seatServiceMock.Object;

            // Act
            var actualList = await seatService.GetAllAsync(2);
            listSeat.RemoveAt(2);

            // Assert
            CollectionAssert.AreEquivalent(listSeat, actualList);
        }

        [Test]
        public void Add_WhenSeatIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
                SeatValidationProvider.IsValidToAdd(null), MESSAGE_EXCEPTION_ADD_NULL);
        }

        [Test]
        public void AddUpdate_WhenSeatAreaIdFailed_ReturnException()
        {
            // Arrange
            var seatInvalidArea = new Seat { Id = 1, Number = 1, Row = 2 };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => SeatValidationProvider.IsValidToAdd(seatInvalidArea), MESSAGE_EXCEPTION_AREA_ID);
            Assert.Throws<ArgumentException>(() => SeatValidationProvider.IsValidToUpdate(seatInvalidArea), MESSAGE_EXCEPTION_AREA_ID);
        }

        [Test]
        public void AddUpdate_WhenSeatNumberFailed_ReturnException()
        {
            // Arrange        
            var seatInvalidNumber = new Seat { AreaId = 1, Number = -2, Row = 4 };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => SeatValidationProvider.IsValidToAdd(seatInvalidNumber), MESSAGE_EXCEPTION_NUMBER);
            Assert.Throws<ArgumentException>(() => SeatValidationProvider.IsValidToUpdate(seatInvalidNumber), MESSAGE_EXCEPTION_NUMBER);
        }

        [Test]
        public void AddUpdate_WhenSeatRowFailed_ReturnException()
        {
            // Arrange          
            var seatInvalidRow = new Seat { AreaId = 3, Number = 2, Row = -1 };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => SeatValidationProvider.IsValidToAdd(seatInvalidRow), MESSAGE_EXCEPTION_ROW);
            Assert.Throws<ArgumentException>(() => SeatValidationProvider.IsValidToUpdate(seatInvalidRow), MESSAGE_EXCEPTION_ROW);
        }
      
        [Test]
        public async Task Add_WhenSeatValidationProviderly_AddAndReturnTrue()
        {
            // Arrange
            var seatList = new List<Seat>();
            var seatServiceMock = new Mock<IServiceBase<Seat>>();
            seatServiceMock.Setup(m => m.AddAsync(It.IsAny<Seat>()))
                .Callback<Seat>(seat => seatList.Add(seat))
                .Returns(Task.FromResult(true));

            var seat = new Seat { AreaId = 2, Number = 1, Row = 1 };

            // Act
            bool result = await seatServiceMock.Object.AddAsync(seat);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(seatList, new List<Seat> { seat });
        }

        [Test]
        public void Update_WhenSeatIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => 
                SeatValidationProvider.IsValidToUpdate(null), MESSAGE_EXCEPTION_UPDATE_NULL);
        }

        [Test]
        public void Update_WhenSeatIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                SeatValidationProvider.IsValidToUpdate(new Seat { Id = -4 }), MESSAGE_EXCEPTION_ID);
        }

        [Test]
        public async Task Update_WhenSeatValidationProviderly_UpdateAndReturnTrue()
        {
            // Arrange
            var seat = new Seat { Id = 1, AreaId = 2, Number = 1, Row = 1 };
            var seatList = new List<Seat> { seat };

            var seatServiceMock = new Mock<IServiceBase<Seat>>();
            seatServiceMock.Setup(m => m.UpdateAsync(It.IsAny<Seat>())).Callback<Seat>(seat =>
            {
                seatList.RemoveAt(0);
                seatList.Add(seat);
            }).Returns(Task.FromResult(true));

            // Act
            seat.Number = 10;
            bool result = await seatServiceMock.Object.UpdateAsync(seat);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.AreEquivalent(seatList, new List<Seat> { seat });
        }

        [Test]
        public void Delete_WhenSeatIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                SeatValidationProvider.IsValidToDelete(0), MESSAGE_EXCEPTION_SEAT_ID);
        }

        [Test]
        public async Task Delete_WnenSeatValidationProviderly_DeleteAndReturnTrue()
        {
            // Arrange
            var seat = new Seat { Id = 0, AreaId = 2, Number = 1, Row = 1 };
            var seatList = new List<Seat> { seat };

            var seatServiceMock = new Mock<IServiceBase<Seat>>();
            seatServiceMock.Setup(m => m.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(seat => seatList.RemoveAt(seat))
                .Returns(Task.FromResult(true));

            // Act
            bool result = await seatServiceMock.Object.DeleteAsync(seat.Id);

            // Assert
            Assert.IsTrue(result);
            CollectionAssert.IsEmpty(seatList);
        }
    }
}
