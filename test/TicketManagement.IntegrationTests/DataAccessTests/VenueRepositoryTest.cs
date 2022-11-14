using System;
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
    /// Testing venueRepo use test database.
    /// </summary>
    public class VenueRepositoryTest
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
        public async Task GetAllVenue_WhenCallAllArea_ReturnEquivaletCollection()
        {
            // Arrange
            var venue = VenueData.GetOne();
            var venueRepo = new SqlVenueRepository(_connectionString);

            // Act
            var resultList = await venueRepo.GetByNameAsync(venue.Name);

            // Assert
            venue.Should().BeEquivalentTo(resultList, options => options.Excluding(x => x.Id));
        }

        [Test]
        public async Task InsertDeleteVenue_AddAndDeleteSeat_ReturnsTrue()
        {
            // Arrange
            var venue = new Venue { Name = "TestName", Description = "TestVenue", Address = "Buda", Phone = "+125667" };
            var venueRepo = new SqlVenueRepository(_connectionString);

            // Act
            var succesfullAdd = await venueRepo.AddAsync(venue);
            var lastVenue = await venueRepo.GetByNameAsync(venue.Name);
            var succesfullDelete = await venueRepo.DeleteAsync(lastVenue.Id);

            // Assert
            venue.Should().BeEquivalentTo(lastVenue, options => options.Excluding(x => x.Id));
            succesfullAdd.Should().BeTrue();
            succesfullDelete.Should().BeTrue();
        }

        [Test]
        public async Task InsertDeleteVenue_AddCheckDeleteCheck_ReturnTrueAndSameCollections()
        {
            // Arrange
            var venue = new Venue { Name = "TestName", Description = "TestVenue", Address = "Buda", Phone = "+125667" };
            var venueRepo = new SqlVenueRepository(_connectionString);

            // Act
            await venueRepo.AddAsync(venue);
            var checkVenue = await venueRepo.GetByNameAsync(venue.Name);
            await venueRepo.DeleteAsync(checkVenue.Id);
            var resultVenue = await venueRepo.GetByNameAsync(venue.Name);

            // Assert
            resultVenue.Should().BeNull();
            checkVenue.Should().NotBeEquivalentTo(resultVenue);
        }

        [Test]
        public async Task UpdateVenue_WhenChangeExistingVenue_ChangesSeat()
        {
            // Arrange
            var venue = new Venue { Id = VenueData.GetOne().Id, Name = "Dance", Address = "Homel", Description = "Well dance", Phone = "+375291234567" }; // How in VenueData
            venue.Name = "New venue_TEST name";
            var venueRepo = new SqlVenueRepository(_connectionString);

            // Act
            var succesfullUp = await venueRepo.UpdateAsync(venue);
            var seatUp = await venueRepo.GetByNameAsync(venue.Name);
            await venueRepo.UpdateAsync(VenueData.GetVenues[0]); // Return old entity

            // Assert
            venue.Should().BeEquivalentTo(seatUp);
            succesfullUp.Should().BeTrue();
        }

        [Test]
        public async Task DeleteUpdateVenue_WhenID_0_ReturnsFalse()
        {
            // Arrange
            var venue = new Venue { Id = 5, Name = "Dance", Address = "Homel", Description = "Well dance", Phone = "+375291234567" };
            venue.Id = 0;
            venue.Name = "UniqueTESTname";
            var venueRepo = new SqlVenueRepository(_connectionString);

            // Act
            Func<Task> actionDel = async () => await venueRepo.DeleteAsync(venue.Id);
            var succesfullUp = await venueRepo.UpdateAsync(venue);

            // Assert
            actionDel.Should().Throw<ArgumentException>()
                .WithMessage("Venue id cannot be 0 or less.");
            succesfullUp.Should().BeFalse();
        }
    }
}
