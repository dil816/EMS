using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Attendees.CreateAttendee;
using EMS.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EMS.Modules.Attendance.Presentation.Attendees;
public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new CreateAttendeeCommand(
                context.Message.UserId,
                context.Message.Email,
                context.Message.FirstName,
                context.Message.LastName),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateAttendeeCommand), result.Error);
        }
    }
}
