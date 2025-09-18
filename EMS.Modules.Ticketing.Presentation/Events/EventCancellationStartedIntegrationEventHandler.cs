using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Events.IntegrationEvents;
using EMS.Modules.Ticketing.Application.Events.CancelEvent;
using MediatR;

namespace EMS.Modules.Ticketing.Presentation.Events;
internal sealed class EventCancellationStartedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventCancellationStartedIntegrationEvent>
{
    public override async Task Handle(
        EventCancellationStartedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new CancelEventCommand(integrationEvent.EventId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CancelEventCommand), result.Error);
        }
    }
}

