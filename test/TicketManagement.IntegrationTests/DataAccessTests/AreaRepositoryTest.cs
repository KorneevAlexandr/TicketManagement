using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Repositories;
using TicketManagement.IntegrationTests.Data;

namespace TicketManagement.IntegrationTests.TestDALProviders
{
    /// <summary>
    /// Testing areaRepo use test database.
    /// </summary>
	public class AreaRepositoryTest
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
        public async Task GetAllArea_WhenCallAll_ReturnEquivaletCollection()
        {
            // Arrange
            var areaList = AreaData.GetById(LayoutData.GetLayouts[0].Id);
            var areaRepo = new SqlAreaRepository(_connectionString);

            // Act
            var resultList = await areaRepo.GetAllAsync(LayoutData.GetLayouts[0].Id);

            // Assert
            areaList.Should().BeEquivalentTo(resultList, options => options.Excluding(x => x.Id));
        }

        [Test]
        public async Task InsertDeleteArea_AddAndDeleteSeat_ReturnsTrue()
        {
            // Arrange
            var area = new Area { LayoutId = LayoutData.GetLayouts[0].Id, Description = "Very Well", CoordX = 10, CoordY = 10 };
            var areaRepo = new SqlAreaRepository(_connectionString);

            // Act
            var succesfullAdd = await areaRepo.AddAsync(area);
            var lastAreas = await areaRepo.GetAllAsync(area.LayoutId);
            var lastArea = lastAreas.Last();
            var succesfullDelete = await areaRepo.DeleteAsync(lastArea.Id);

            // Assert
            area.Should().BeEquivalentTo(lastArea, options => options.Excluding(x => x.Id));
            succesfullAdd.Should().BeTrue();
            succesfullDelete.Should().BeTrue();
        }

        [Test]
        public async Task InsertDeleteArea_AddCheckDeleteCheck_ReturnTrueAndSameCollections()
        {
            // Arrange
            var layoutId = LayoutData.GetLayouts[0].Id;
            var defaultList = AreaData.GetById(layoutId);
            var area = new Area { LayoutId = layoutId, Description = "Very Well", CoordX = 10, CoordY = 10 };
            var areaRepo = new SqlAreaRepository(_connectionString);

            // Act
            await areaRepo.AddAsync(area);
            var checkList = await areaRepo.GetAllAsync(layoutId);
            await areaRepo.DeleteAsync(checkList.Last().Id);
            var resultList = await areaRepo.GetAllAsync(layoutId);

            // Assert
            resultList.Should().BeEquivalentTo(defaultList, options => options.Excluding(x => x.Id));
            checkList.Should().NotBeEquivalentTo(resultList);
        }

        [Test]
        public async Task UpdateArea_WhenChangeExistingArea_ChangesSeat()
        {
            // Arrange
            var area = new Area { Id = AreaData.GetAreas[1].Id, LayoutId = LayoutData.GetLayouts[0].Id, CoordX = 2, CoordY = 2, Description = "High quality" };
            area.CoordX = 10;
            var areaRepo = new SqlAreaRepository(_connectionString);

            // Act
            var succesfullUp = await areaRepo.UpdateAsync(area);
            var seatsUp = await areaRepo.GetAllAsync(area.LayoutId);
            var seatUp = seatsUp.FirstOrDefault(x => x.Id == area.Id);
            await areaRepo.UpdateAsync(AreaData.GetAreas[1]); // Return old entity

            // Assert
            area.Should().BeEquivalentTo(seatUp);
            succesfullUp.Should().BeTrue();
        }

        [Test]
        public async Task DeleteUpdateArea_WhenID_0_ReturnsFalse()
        {
            // Arrange
            var area = new Area { LayoutId = 5, Description = "Empty", CoordX = 1, CoordY = 1 };
            area.Id = 0;
            var areaRepo = new SqlAreaRepository(_connectionString);

            // Act
            Func<Task> actionDel = async () => await areaRepo.DeleteAsync(area.Id);
            var succesfullUp = await areaRepo.UpdateAsync(area);

            // Assert
            actionDel.Should().Throw<ArgumentException>()
                .WithMessage("Area id cannot be 0 or less.");
            succesfullUp.Should().BeFalse();
        }
    }
}
