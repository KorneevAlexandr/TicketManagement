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
    /// Testing layoutRepo use test database.
    /// </summary>
    public class LayoutRepositoryTest
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
        public async Task GetAllLayout_WhenCallAll_ReturnEquivaletCollection()
        {
            // Arrange
            var layoutList = LayoutData.GetLayouts;
            var layoutRepo = new SqlLayoutRepository(_connectionString);

            // Act
            var resultList = await layoutRepo.GetAllAsync(VenueData.GetOne().Id);

            // Assert
            layoutList.Should().BeEquivalentTo(resultList, options => options.Excluding(x => x.Id));
        }

        [Test]
        public async Task InsertDeleteLayout_AddAndDeleteSeat_ReturnsTrue()
        {
            // Arrange
            var layout = new Layout { Name = "Uniq Name ", Description = "Desc", VenueId = VenueData.GetOne().Id };
            var layoutRepo = new SqlLayoutRepository(_connectionString);

            // Act
            var succesfullAdd = await layoutRepo.AddAsync(layout);
            var allLastLayout = await layoutRepo.GetAllAsync(layout.VenueId);
            var lastLayout = allLastLayout.Last();
            var succesfullDelete = await layoutRepo.DeleteAsync(lastLayout.Id);

            // Assert
            layout.Should().BeEquivalentTo(lastLayout, options => options.Excluding(x => x.Id));
            succesfullAdd.Should().BeTrue();
            succesfullDelete.Should().BeTrue();
        }

        [Test]
        public async Task InsertDeleteLayout_AddCheckDeleteCheck_ReturnTrueAndSameCollections()
        {
            // Arrange
            var defaultList = LayoutData.GetLayouts;
            var layout = new Layout { Name = "Uniq Name ", Description = "Desc", VenueId = VenueData.GetOne().Id };
            var layoutRepo = new SqlLayoutRepository(_connectionString);

            // Act
            await layoutRepo.AddAsync(layout);
            var checkList = await layoutRepo.GetAllAsync(layout.VenueId);
            await layoutRepo.DeleteAsync(checkList.Last().Id);
            var resultList = await layoutRepo.GetAllAsync(layout.VenueId);

            // Assert
            resultList.Should().BeEquivalentTo(defaultList, options => options.Excluding(x => x.Id));
            checkList.Should().NotBeEquivalentTo(resultList);
        }

        [Test]
        public async Task UpdateLayout_WhenChangeExistingLayout_ChangesSeat()
        {
            // Arrange
            var layout = new Layout { Id = LayoutData.GetLayouts[0].Id, Name = "Hol 1", VenueId = VenueData.GetOne().Id, Description = "First layout" }; // How 1-st in LayoutData
            layout.Name = "New test_layout name";
            var layoutRepo = new SqlLayoutRepository(_connectionString);

            // Act
            var succesfullUp = await layoutRepo.UpdateAsync(layout);
            var allSeatsUp = await layoutRepo.GetAllAsync(layout.VenueId);
            var seatUp = allSeatsUp.FirstOrDefault(x => x.Id == layout.Id);
            await layoutRepo.UpdateAsync(LayoutData.GetLayouts[0]); // Return old entity

            // Assert
            layout.Should().BeEquivalentTo(seatUp);
            succesfullUp.Should().BeTrue();
        }

        [Test]
        public async Task DeleteUpdateLayout_WhenID_0_ReturnsFalse()
        {
            // Arrange
            var layout = new Layout { Name = "Not uniq name", Description = "Empty", VenueId = 5 };
            layout.Id = 0;
            var layoutRepo = new SqlLayoutRepository(_connectionString);

            // Act
            Func<Task> actionDel = async () => await layoutRepo.DeleteAsync(layout.Id);
            var succesfullUp = await layoutRepo.UpdateAsync(layout);

            // Assert
            actionDel.Should().Throw<ArgumentException>()
                .WithMessage("Layout id cannot be 0 or less.");
            succesfullUp.Should().BeFalse();
        }
    }
}
