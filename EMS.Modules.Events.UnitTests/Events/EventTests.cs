using EMS.Common.Domain;
using EMS.Modules.Events.Domain.Categories;
using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.UnitTests.Abstraction;
using FluentAssertions;

namespace EMS.Modules.Events.UnitTests.Events;
public class EventTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenEndDatePrecedesStartDate()
    {
        // Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;
        DateTime endsAtUtc = startAtUtc.AddMinutes(-1);

        // Act
        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startAtUtc,
            endsAtUtc);

        // Assert
        result.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
    {
        // Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startAtUtc = DateTime.UtcNow;

        // Act
        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startAtUtc,
            null);

        Event @event = result.Value;

        // Assert
        EventCreatedDomainEvent domainEvent = BaseTest.AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event);
        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Publish_ShouldReturnFailure_WhenEventNotDraft()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        @event.Publish();

        //Act
        Result publishResult = @event.Publish();

        //Assert
        publishResult.Error.Should().Be(EventErrors.NotDraft);
    }

    [Fact]
    public void Publish_ShouldRaiseDomainEvent_WhenEventPublished()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        @event.Publish();

        //Assert
        EventPublishedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventPublishedDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        @event.Reschedule(startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

        //Assert
        EventRescheduledDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        @event.Cancel(startsAtUtc.AddMinutes(-1));

        //Assert
        EventCanceledDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventCanceledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyCanceled()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        @event.Cancel(startsAtUtc.AddMinutes(-1));

        //Act
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(-1));

        //Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyCanceled);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        //Act
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(1));

        //Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyStarted);
    }
}
