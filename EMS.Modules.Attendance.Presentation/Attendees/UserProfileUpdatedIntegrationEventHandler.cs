using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Attendees.UpdateAttendee;
using EMS.Modules.Users.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Attendance.Presentation.Attendees;
internal sealed class UserProfileUpdatedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(UserProfileUpdatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(UpdateAttendeeCommand), result.Error);
        }
    }
}
