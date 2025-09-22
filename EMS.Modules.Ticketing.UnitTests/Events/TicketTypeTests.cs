using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Ticketing.UnitTests.Events;
public class TicketTypeTests
{
    [Fact]
    public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Act
        Result<TicketType> result = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Random.Decimal(),
            BaseTest.Faker.Random.String(3),
            BaseTest.Faker.Random.Decimal());

        //Assert
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void UpdateQuantity_ShouldReturnFailure_WhenNotEnoughQuanitity()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        decimal quantity = BaseTest.Faker.Random.Decimal();
        var ticketType = TicketType.Create(
        Guid.NewGuid(),
        @event.Id,
        BaseTest.Faker.Name.FirstName(),
        BaseTest.Faker.Random.Decimal(),
        BaseTest.Faker.Random.String(3),
        quantity);

        //Act
        Result result = ticketType.UpdateQuantity(quantity + 1);

        //Assert
        result.Error.Should().Be(TicketTypeErrors.NotEnoughQuantity(quantity));
    }

    [Fact]
    public void UpdateQuantity_ShouldRaiseDomainEvent_WhenTicketTypesIsSoldOut()
    {
        //Arrange
        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        decimal quantity = BaseTest.Faker.Random.Decimal();
        Result<TicketType> ticketType = TicketType.Create(
        Guid.NewGuid(),
        @event.Id,
        BaseTest.Faker.Name.FirstName(),
        BaseTest.Faker.Random.Decimal(),
        BaseTest.Faker.Random.String(3),
        quantity);

        //Act
        ticketType.Value.UpdateQuantity(quantity);

        //Assert
        TicketTypeSoldOutDomainEvent domainEvent = BaseTest.AssertDomainEventWasPublished<TicketTypeSoldOutDomainEvent>(ticketType.Value);

        domainEvent.TicketTypeId.Should().Be(ticketType.Value.Id);
    }
}

