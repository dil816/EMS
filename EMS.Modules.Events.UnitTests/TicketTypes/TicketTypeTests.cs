using EMS.Common.Domain;
using EMS.Modules.Events.Domain.Categories;
using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.Domain.TicketTypes;
using EMS.Modules.Events.UnitTests.Abstraction;
using FluentAssertions;

namespace EMS.Modules.Events.UnitTests.TicketTypes;
public class TicketTypeTests
{
    [Fact]
    public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> eventResult = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        //Act
        Result<TicketType> result = TicketType.Create(
            eventResult.Value,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Random.Decimal(),
            BaseTest.Faker.Random.String(),
            BaseTest.Faker.Random.Decimal());

        //Assert
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void UpdatePrice_ShouldRaiseDomainEvent_WhenTicketTypeIsUpdated()
    {
        //Arrange
        var category = Category.Create(BaseTest.Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> eventResult = Event.Create(
            category,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Result<TicketType> result = TicketType.Create(
            eventResult.Value,
            BaseTest.Faker.Music.Genre(),
            BaseTest.Faker.Random.Decimal(),
            BaseTest.Faker.Random.String(),
            BaseTest.Faker.Random.Decimal());

        TicketType ticketType = result.Value;

        //Act
        ticketType.UpdatePrice(BaseTest.Faker.Random.Decimal());

        //Assert
        TicketTypePriceChangedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<TicketTypePriceChangedDomainEvent>(ticketType);

        domainEvent.TicketTypeId.Should().Be(ticketType.Id);
    }
}
