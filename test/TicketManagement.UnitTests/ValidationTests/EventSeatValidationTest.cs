using System;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain;
using TicketManagement.BusinessLogic.ValidationProviders;

namespace TicketManagement.UnitTests.TestsBLValidation
{
    /// <summary>
    /// Testing the correctness of passed objects EventSeat to business logic methods.
    /// </summary>
    public class EventSeatValidationTest
	{
        private const string MESSAGE_EXCEPTION_AREA_ID = "Invalid AreaId: such area does not exist.";
        private const string MESSAGE_EXCEPTION_ROW = "Invalid Row: Row cannot be less 1.";
        private const string MESSAGE_EXCEPTION_NUMBER = "Invalid Number: Number cannot be less 1.";
        private const string MESSAGE_EXCEPTION_ID = "Invalid Id: Id cannot be less 1.";
        private const string MESSAGE_EXCEPTION_SEAT_ID = "Seat id cannot be 0 or less.";
        private const string MESSAGE_EXCEPTION_UPDATE_NULL = "Can not update null seat. (Parameter 'seat')";

        [Test]
        public void Update_WhenEventSeatAreaIdFailed_ReturnException()
        {
            // Arrange
            var seatInvalidArea = new EventSeat { Id = 1, Number = 1, Row = 2 };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventSeatValidationProvider.IsValidToUpdate(seatInvalidArea), MESSAGE_EXCEPTION_AREA_ID);
        }

        [Test]
        public void Update_WhenEventSeatNumberFailed_ReturnException()
        {
            // Arrange        
            var seatInvalidNumber = new EventSeat { AreaId = 1, Number = -2, Row = 4 };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventSeatValidationProvider.IsValidToUpdate(seatInvalidNumber), MESSAGE_EXCEPTION_NUMBER);
        }

        [Test]
        public void Update_WhenEventSeatRowFailed_ReturnException()
        {
            // Arrange          
            var seatInvalidRow = new EventSeat { AreaId = 3, Number = 2, Row = -1 };

            // Act, Assert
            Assert.Throws<ArgumentException>(() => EventSeatValidationProvider.IsValidToUpdate(seatInvalidRow), MESSAGE_EXCEPTION_ROW);
        }

        [Test]
        public void Update_WhenEventSeatIsNull_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() =>
                EventSeatValidationProvider.IsValidToUpdate(null), MESSAGE_EXCEPTION_UPDATE_NULL);
        }

        [Test]
        public void Update_WhenEventSeatIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                EventSeatValidationProvider.IsValidToUpdate(new EventSeat { Id = -4 }), MESSAGE_EXCEPTION_ID);
        }

        [Test]
        public void Delete_WhenEventSeatIdNotValidly_ReturnException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() =>
                EventSeatValidationProvider.IsValidToDelete(0), MESSAGE_EXCEPTION_SEAT_ID);
        }
    }
}
