using Bogus;
using EMS.Common.Domain;
using EMS.Modules.Events.Application.TicketTypes.GetTicketType;
using EMS.Modules.Events.Domain.TicketTypes;
using EMS.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Events.IntegrationTests.TicketTypes;
public class GetTicketTypeTests : BaseIntegrationTest
{
    public GetTicketTypeTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange
        var query = new GetTicketTypeQuery(Guid.NewGuid());

        // Act
        Result<TicketTypeResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(TicketTypeErrors.NotFound(query.TicketTypeId));
    }

    [Fact]
    public async Task Should_ReturnTicketType_WhenTicketTypeExists()
    {
        // Arrange
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        Guid ticketTypeId = await Sender.CreateTicketTypeAsync(eventId);

        var query = new GetTicketTypeQuery(ticketTypeId);

        // Act
        Result<TicketTypeResponse> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}

