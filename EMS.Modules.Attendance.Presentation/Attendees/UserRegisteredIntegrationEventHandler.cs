using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Attendees.CreateAttendee;
using EMS.Modules.Users.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Attendance.Presentation.Attendees;
public sealed class UserRegisteredIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(
        UserRegisteredIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateAttendeeCommand), result.Error);
        }
    }
}
