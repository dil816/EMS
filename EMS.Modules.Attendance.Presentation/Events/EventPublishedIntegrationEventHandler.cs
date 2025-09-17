using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Events.CreateEvent;
using EMS.Modules.Events.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Attendance.Presentation.Events;
public sealed class EventPublishedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public override async Task Handle(EventPublishedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateEventCommand(
                integrationEvent.EventId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Location,
                integrationEvent.StartsAtUtc,
                integrationEvent.EndsAtUtc),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateEventCommand), result.Error);
        }
    }
}
