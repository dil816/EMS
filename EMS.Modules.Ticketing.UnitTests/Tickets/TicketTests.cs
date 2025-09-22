using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Ticketing.Domain.Orders;
using EMS.Modules.Ticketing.Domain.Tickets;
using EMS.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Ticketing.UnitTests.Tickets;

public class TicketTests
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Name.LastName());

        var order = Order.Create(customer);

        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Random.Decimal(),
            BaseTest.Faker.Random.String(3),
            BaseTest.Faker.Random.Decimal());

        //Act
        Result<Ticket> result = Ticket.Create(
            order,
            ticketType);

        //Assert
        TicketCreatedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenTicketIsArchived()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Name.LastName());

        var order = Order.Create(customer);

        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Random.Decimal(),
            BaseTest.Faker.Random.String(3),
            BaseTest.Faker.Random.Decimal());

        Result<Ticket> result = Ticket.Create(
            order,
            ticketType);

        //Act
        result.Value.Archive();

        //Assert
        TicketArchivedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<TicketArchivedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }
}

