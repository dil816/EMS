using EMS.Common.Domain;
using EMS.Modules.Attendance.Domain.Attendees;
using EMS.Modules.Attendance.Domain.Events;
using EMS.Modules.Attendance.Domain.Tickets;
using EMS.Modules.Attendance.UnitTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Attendance.UnitTests.Tickets;
public class TicketTests
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Person.FirstName,
            BaseTest.Faker.Person.LastName);

        DateTime startsAtUtc = DateTime.UtcNow;

        var @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetName(),
            startsAtUtc, null);

        //Act
        Result<Ticket> result = Ticket.Create(
            Guid.NewGuid(),
            attendee,
            @event,
            BaseTest.Faker.Random.String());

        //Assert
        TicketCreatedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void MarkAsUsed_ShouldRaiseDomainEvent_WhenTicketIsUsed()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Person.FirstName,
            BaseTest.Faker.Person.LastName);

        DateTime startsAtUtc = DateTime.UtcNow;

        var @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetName(),
            startsAtUtc, null);

        var ticket = Ticket.Create(
            Guid.NewGuid(),
            attendee,
            @event,
            BaseTest.Faker.Random.String());

        //Act
        ticket.MarkAsUsed();

        //Assert
        TicketUsedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<TicketUsedDomainEvent>(ticket);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }
}
