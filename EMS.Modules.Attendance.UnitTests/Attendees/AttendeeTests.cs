using EMS.Common.Domain;
using EMS.Modules.Attendance.Domain.Attendees;
using EMS.Modules.Attendance.Domain.Events;
using EMS.Modules.Attendance.Domain.Tickets;
using EMS.Modules.Attendance.UnitTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Attendance.UnitTests.Attendees;
public class AttendeeTests
{
    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketIsNotValid()
    {
        //Arrange
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Person.FirstName,
            BaseTest.Faker.Person.LastName);

        var ticketAttendee = Attendee.Create(
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
            ticketAttendee,
            @event,
            BaseTest.Faker.Random.String());

        //Act
        Result checkInAttendee = attendee.CheckIn(ticket);

        //Assert
        InvalidCheckInAttemptedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<InvalidCheckInAttemptedDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);

        checkInAttendee.Error.Should().Be(TicketErrors.InvalidCheckIn);
    }

    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketAlreadyUsed()
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

        ticket.MarkAsUsed();

        //Act
        Result checkInAttendee = attendee.CheckIn(ticket);

        //Assert
        DuplicateCheckInAttemptedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<DuplicateCheckInAttemptedDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);

        checkInAttendee.Error.Should().Be(TicketErrors.DuplicateCheckIn);
    }

    [Fact]
    public void CheckIn_ShouldRaiseDomainEvent_WhenSuccessfullyCheckedIn()
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
        attendee.CheckIn(ticket);

        //Assert
        AttendeeCheckedInDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<AttendeeCheckedInDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);
    }
}
