using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Ticketing.UnitTests.Events;
public class EventTests
{
    [Fact]
    public void Create_ShouldReturnValue_WhenEventIsCreated()
    {
        //Act
        Result<Event> @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        //Assert
        @event.Value.Should().NotBeNull();
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventIsRescheduled()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Act
        @event.Value.Reschedule(
            startsAtUtc.AddDays(1),
            startsAtUtc.AddDays(2));

        //Assert
        EventRescheduledDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event.Value);

        domainEvent.EventId.Should().Be(@event.Value.Id);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventIsCanceled()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Act
        @event.Value.Cancel();

        //Assert
        EventCanceledDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventCanceledDomainEvent>(@event.Value);

        domainEvent.EventId.Should().Be(@event.Value.Id);
    }

    [Fact]
    public void PaymentsRefunded_ShouldRaiseDomainEvent_WhenPaymentsAreRefunded()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Act
        @event.Value.PaymentsRefunded();

        //Assert
        EventPaymentsRefundedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventPaymentsRefundedDomainEvent>(@event.Value);

        domainEvent.EventId.Should().Be(@event.Value.Id);

    }

    [Fact]
    public void TicketsArchived_ShouldRaiseDomainEvent_WhenTicketsAreArchived()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Act
        @event.Value.TicketsArchived();

        //Assert
        EventTicketsArchivedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventTicketsArchivedDomainEvent>(@event.Value);

        domainEvent.EventId.Should().Be(@event.Value.Id);

    }
}

