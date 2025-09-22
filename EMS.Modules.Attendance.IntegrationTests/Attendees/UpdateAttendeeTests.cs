using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Attendees.UpdateAttendee;
using EMS.Modules.Attendance.Domain.Attendees;
using EMS.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Attendance.IntegrationTests.Attendees;

public class UpdateAttendeeTests : BaseIntegrationTest
{
    public UpdateAttendeeTests(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange
        var command = new UpdateAttendeeCommand(
            Guid.NewGuid(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenAttendeeExists()
    {
        // Arrange
        Guid attendeeId = await Sender.CreateAttendeeAsync(Guid.NewGuid());

        var command = new UpdateAttendeeCommand(
            attendeeId,
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}

