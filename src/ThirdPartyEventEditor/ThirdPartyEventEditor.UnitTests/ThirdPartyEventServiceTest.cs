using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Exceptions;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Repositories;
using ThirdPartyEventEditor.Services;

namespace ThirdPartyEventEditor.UnitTests
{
	/// <summary>
	/// Provides unit tests to validate the health of a ThirdPatryEventService,
	/// that has the business logic for managing the ThirdPartyEvent entity
	/// </summary>
	[TestFixture]
	public class ThirdPartyEventServiceTest
	{
		[Test]
		public async Task GetAllAsync_WhenCollectionIsNonEmpty_ReturnCollection()
		{
			// Arrange
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync()).ReturnsAsync(EventCollection.Events);
			var service = new ThirdPartyEventService(repositoryMock.Object);
			var expect = EventCollection.Events;

			// Act
			var actual = await service.GetAllAsync();

			// Assert
			expect.Should().BeEquivalentTo(actual);
		}

		[Test]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public async Task GetAsync_WhenIdIsExist_ReturnThirdPartyEvent(int id)
		{
			// Arrange
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAsync(id))
				.ReturnsAsync(EventCollection.Events.FirstOrDefault(partyEvent => partyEvent.Id == id));
			var service = new ThirdPartyEventService(repositoryMock.Object);
			var expected = EventCollection.Events.FirstOrDefault(partyEvent => partyEvent.Id == id);

			// Act
			var actual = await service.GetAsync(id);

			// Assert
			expected.Should().BeEquivalentTo(actual);
		}

		[Test]
		[TestCase(-5)]
		[TestCase(0)]
		public async Task GetAsync_WhenIdIsNotExist_ReturnException(int id)
		{
			// Arrange
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAsync(id))
				.ReturnsAsync(EventCollection.Events.FirstOrDefault(partyEvent => partyEvent.Id == id));
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act
			Func<Task> action = async () => await service.GetAsync(id);

			// Assert
			await action.Should().ThrowAsync<EventManagementException>()
				.WithMessage("Id can not be less or equal to 0.");
		}

		[Test]
		[TestCase(3)]
		[TestCase(2)]
		[TestCase(1)]
		public async Task DeleteAsync_WhenIdIsValid_ReturnSameEvents(int id)
		{
			// Arrange
			var expectedEvents = new List<ThirdPartyEvent>
			{
				new ThirdPartyEvent { Id = 1 },
				new ThirdPartyEvent { Id = 2 },
				new ThirdPartyEvent { Id = 3 },
			};
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync()).ReturnsAsync(expectedEvents);
			repositoryMock.Setup(method => method.DeleteAsync(id))
				.Callback(() =>
				{
					expectedEvents.RemoveAt(id - 1);
				});
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act, Assert
			await service.DeleteAsync(id);
			var result = await service.GetAllAsync();
			expectedEvents.Should().BeEquivalentTo(result);
		}

		[Test]
		public async Task AddAsync_WhenEventIsValid_ReturnCompleteCollection()
		{
			// Arrange
			var expectedEvents = EventCollection.Events;
			var addedEvent = EventCollection.DopEvent;
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync()).ReturnsAsync(expectedEvents);
			repositoryMock.Setup(method => method.AddAsync(addedEvent))
				.Callback<ThirdPartyEvent>(partyEvent =>
				{
					expectedEvents.Add(partyEvent);
				});
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act, Assert
			await service.AddAsync(addedEvent);
			var result = await service.GetAllAsync();
			result.Last().Should().BeEquivalentTo(addedEvent);
		}

		[Test]
		public async Task AddAsync_WnenNameIsNotUnique_ReturnException()
		{
			// Arrange
			var expectedEvents = EventCollection.Events;
			var addedEvent = EventCollection.DopEvent;
			// create not unique name
			addedEvent.Name = expectedEvents.First().Name;

			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync()).ReturnsAsync(expectedEvents);
			repositoryMock.Setup(method => method.AddAsync(addedEvent))
				.Callback<ThirdPartyEvent>(partyEvent =>
				{
					expectedEvents.Add(partyEvent);
				});
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act
			Func<Task> func = () => service.AddAsync(addedEvent);

			// Assert
			await func.Should().ThrowAsync<EventManagementException>()
				.WithMessage($"Name '{addedEvent.Name}' is not unique.");
		}


		[Test]
		public async Task AddAsync_WnenStartDateAfterEndDate_ReturnException()
		{
			// Arrange
			var expectedEvents = EventCollection.Events;
			var addedEvent = new ThirdPartyEvent
			{
				Name = "Good Name",
				EndDate = new DateTime(2010, 10, 19),
				StartDate = DateTime.MaxValue,
			};
			
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync()).ReturnsAsync(expectedEvents);
			repositoryMock.Setup(method => method.AddAsync(addedEvent))
				.Callback<ThirdPartyEvent>(partyEvent =>
				{
					expectedEvents.Add(partyEvent);
				});
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act
			Func<Task> func = () => service.AddAsync(addedEvent);

			// Assert
			await func.Should().ThrowAsync<EventManagementException>()
				.WithMessage("Start date cannot be later than end date.");
		}

		[Test]
		public async Task AddAsync_WnenDateInPast_ReturnException()
		{
			// Arrange
			var expectedEvents = EventCollection.Events;
			var addedEvent = new ThirdPartyEvent
			{
				Name = "Good Name",
				EndDate = new DateTime(2010, 10, 19),
				StartDate = new DateTime(2010, 10, 18),
			};

			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync()).ReturnsAsync(expectedEvents);
			repositoryMock.Setup(method => method.AddAsync(addedEvent))
				.Callback<ThirdPartyEvent>(partyEvent =>
				{
					expectedEvents.Add(partyEvent);
				});
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act
			Func<Task> func = () => service.AddAsync(addedEvent);

			// Assert
			await func.Should().ThrowAsync<EventManagementException>()
				.WithMessage("Event can not created in past.");
		}

		[Test]
		public async Task UpdateAsync_WhenEventIsValid_ReturnUpdatedEvent()
		{
			// Arrange
			var eventForCollection = new ThirdPartyEvent
			{
				Id = 1,
				StartDate = DateTime.Now,
				EndDate = DateTime.MaxValue,
				Name = "Alex",
				Description = "For change name",
			};
			var eventForUpdate = new ThirdPartyEvent
			{
				Id = 1,
				Name = "New name!",
				StartDate = DateTime.Now,
				EndDate = DateTime.MaxValue,
				Description = "For change name",
			};
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync())
				.ReturnsAsync(new List<ThirdPartyEvent> { eventForCollection });
			repositoryMock.Setup(method => method.GetAsync(It.IsAny<int>()))
				.ReturnsAsync(eventForCollection);
			repositoryMock.Setup(method => method.UpdateAsync(eventForUpdate))
				.Callback(() =>
				{
					eventForCollection.Name = eventForUpdate.Name;
				});
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act, Assert
			await service.UpdateAsync(eventForUpdate);
			var result = await service.GetAllAsync();
			result.Last().Should().BeEquivalentTo(eventForUpdate);		
		}

		[Test]
		public async Task UpdateAsync_WhenEventDateStartAfterDateEnd_ReturnException()
		{
			// Arrange
			var eventForCollection = new ThirdPartyEvent
			{
				Id = 1,
				StartDate = DateTime.Now,
				EndDate = DateTime.MaxValue,
				Name = "Alex",
				Description = "For change name",
			};
			var eventForUpdate = new ThirdPartyEvent
			{
				Id = 1,
				Name = "Alex",
				StartDate = DateTime.MaxValue, // this is difference
				EndDate = DateTime.Now,
				Description = "This is other description, for test!",
			};
			var repositoryMock = new Mock<IThirdPartyEventRepository>();
			repositoryMock.Setup(method => method.GetAllAsync())
				.ReturnsAsync(new List<ThirdPartyEvent> { eventForCollection });
			repositoryMock.Setup(method => method.GetAsync(It.IsAny<int>()))
				.ReturnsAsync(eventForCollection);
			repositoryMock.Setup(method => method.UpdateAsync(eventForUpdate))
				.Callback(() =>
				{
					eventForCollection.Name = eventForUpdate.Name;
				});
			var service = new ThirdPartyEventService(repositoryMock.Object);

			// Act
			Func<Task> func = () => service.UpdateAsync(eventForUpdate);

			// Assert
			await func.Should().ThrowAsync<EventManagementException>()
				.WithMessage("Start date cannot be later than end date.");
		}
	}
}
