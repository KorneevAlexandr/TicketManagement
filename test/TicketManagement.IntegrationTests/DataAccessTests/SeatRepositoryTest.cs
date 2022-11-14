using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Repositories;
using TicketManagement.DataAccess.Domain;
using FluentAssertions;
using TicketManagement.IntegrationTests.Data;

namespace TicketManagement.IntegrationTests.TestDALProviders
{
    /// <summary>
    /// Testing seatRepo use test database.
    /// </summary>
    public class SeatRepositoryTest
	{
        private string _connectionString;

        /// <summary>
        /// Retrieving and Initializing the test database connection string.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json").Build();

            _connectionString = config.GetConnectionString("TestConnectionString");
        }

        [Test]
        public async Task GetAllSeat_WhenCallGetAll_ReturnEquivaletCollection()
        {
            // Arrange
            var id = 1;
            var seatList = SeatData.GetById(id);
            var seatRepo = new SqlSeatRepository(_connectionString);

            // Act
            var resultList = await seatRepo.GetAllAsync(id);

            // Assert
            seatList.Should().BeEquivalentTo(resultList, options => options.Excluding(x => x.Id));
        }

        [Test]
        public async Task InsertDeleteSeat_AddAndDeleteSeat_ReturnsTrue()
        {
            // Arrange
            var seat = new Seat { AreaId = AreaData.GetAreas[0].Id, Number = 3, Row = 3 };
            var seatRepo = new SqlSeatRepository(_connectionString);

            // Act
            var succesfullAdd = await seatRepo.AddAsync(seat);
            var allLastSeats = await seatRepo.GetAllAsync(seat.AreaId);
            var lastSeat = allLastSeats.Last();
            var succesfullDelete = await seatRepo.DeleteAsync(lastSeat.Id);

            // Assert
            seat.Should().BeEquivalentTo(lastSeat, options => options.Excluding(x => x.Id));
            succesfullAdd.Should().BeTrue();
            succesfullDelete.Should().BeTrue();
        }

        [Test]
        public async Task InsertDeleteSeat_AddCheckDeleteCheck_ReturnTrueAndSameCollections()
        {
            // Arrange
            var seat = new Seat { AreaId = AreaData.GetAreas[0].Id, Number = 3, Row = 3 };
            var defaultList = SeatData.GetById(seat.AreaId);
            var seatRepo = new SqlSeatRepository(_connectionString);

            // Act
            await seatRepo.AddAsync(seat);
            var checkList = await seatRepo.GetAllAsync(seat.AreaId);
            await seatRepo.DeleteAsync(checkList.Last().Id);
            var resultList = await seatRepo.GetAllAsync(seat.AreaId);

            // Assert
            resultList.Should().BeEquivalentTo(defaultList, options => options.Excluding(x => x.Id));
            checkList.Should().NotBeEquivalentTo(resultList);
        }

        [Test]
        public async Task UpdateSeat_WhenChangeExistingSeat_ChangesSeat()
        {
            // Arrange
            var seat = new Seat { Id = SeatData.GetSeats[0].Id, AreaId = AreaData.GetAreas[0].Id, Number = 1, Row = 1 }; // How 1-st in SeatData
            seat.Row = 10;
            var seatRepo = new SqlSeatRepository(_connectionString);

            // Act
            var succesfullUp = await seatRepo.UpdateAsync(seat);
            var seatUpTask = await seatRepo.GetAllAsync(seat.AreaId);
            var seatUp = seatUpTask.FirstOrDefault(x => x.Id == seat.Id);
            await seatRepo.UpdateAsync(SeatData.GetSeats[0]); // Return old entity

            // Assert
            seat.Should().BeEquivalentTo(seatUp);
            succesfullUp.Should().BeTrue();
        }

        [Test]
        public async Task DeleteUpdateSeat_WhenID_0_ReturnsFalse()
        {
            // Arrange
            var seat = new Seat { AreaId = 5, Number = 1, Row = 1 };
            seat.Id = 0;
            var seatRepo = new SqlSeatRepository(_connectionString);

            // Act
            Func<Task> actionDel = async () => await seatRepo.DeleteAsync(seat.Id);
            var succesfullUp = await seatRepo.UpdateAsync(seat);

            // Assert
            actionDel.Should().Throw<ArgumentException>()
                .WithMessage("Seat id cannot be 0 or less.");
            succesfullUp.Should().BeFalse();
        }
    }
}
