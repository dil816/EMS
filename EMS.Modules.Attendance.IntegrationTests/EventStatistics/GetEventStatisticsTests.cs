using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Events.EventStatistics.GetEventStatistics;
using EMS.Modules.Attendance.Domain.Events;
using EMS.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Attendance.IntegrationTests.EventStatistics;
public class GetEventStatisticsTests : BaseIntegrationTest
{
    public GetEventStatisticsTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventStatisticsDoesNotExist()
    {
        // Arrange
        var query = new GetEventStatisticsQuery(Guid.NewGuid());

        // Act
        Result<EventStatisticsResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(EventErrors.NotFound(query.EventId));
    }
}
