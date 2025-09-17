using EMS.Common.Application.EventBus;
using EMS.Common.Application.Messaging;
using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.IntegrationEvents;

namespace EMS.Modules.Events.Application.Events.RescheduleEvent;
internal sealed class EventRescheduledDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<EventRescheduledDomainEvent>
{
    public override async Task Handle(
        EventRescheduledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventRescheduledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc),
            cancellationToken);
    }
}

