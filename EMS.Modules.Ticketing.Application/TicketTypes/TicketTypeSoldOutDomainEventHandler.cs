using EMS.Common.Application.EventBus;
using EMS.Common.Application.Messaging;
using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Ticketing.IntegrationEvents;

namespace EMS.Modules.Ticketing.Application.TicketTypes;
internal sealed class TicketTypeSoldOutDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<TicketTypeSoldOutDomainEvent>
{
    public override async Task Handle(
        TicketTypeSoldOutDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new TicketTypeSoldOutIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.TicketTypeId),
            cancellationToken);
    }
}
