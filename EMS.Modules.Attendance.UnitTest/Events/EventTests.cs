using Bogus;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Domain.Events;
using EMS.Modules.Attendance.UnitTest.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Attendance.UnitTest.Events;
public class EventTests
{
    [Fact]
    public void Should_RaiseDomainEvent_WhenEventCreated()
    {
        //Arrange
        var eventId = Guid.NewGuid();
        DateTime startsAtUtc = DateTime.Now;

        //Act
        Result<Event> result = Event.Create(
            eventId,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Assert
        EventCreatedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<EventCreatedDomainEvent>(result.Value);

        domainEvent.EventId.Should().Be(result.Value.Id);
    }
}
