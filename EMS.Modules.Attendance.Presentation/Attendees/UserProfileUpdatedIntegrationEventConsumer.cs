using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Attendees.UpdateAttendee;
using EMS.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EMS.Modules.Attendance.Presentation.Attendees;
public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserProfileUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserProfileUpdatedIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new UpdateAttendeeCommand(
                context.Message.UserId,
                context.Message.FirstName,
                context.Message.LastName),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(UpdateAttendeeCommand), result.Error);
        }
    }
}
