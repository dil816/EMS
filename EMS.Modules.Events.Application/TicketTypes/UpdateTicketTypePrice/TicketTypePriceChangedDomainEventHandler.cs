using EMS.Common.Application.EventBus;
using EMS.Common.Application.Messaging;
using EMS.Modules.Events.Domain.TicketTypes;
using EMS.Modules.Events.IntegrationEvents;

namespace EMS.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
internal sealed class TicketTypePriceChangedDomainEventHandler(IEventBus eventBus)
     : DomainEventHandler<TicketTypePriceChangedDomainEvent>
{
    public override async Task Handle(
        TicketTypePriceChangedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new TicketTypePriceChangedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.TicketTypeId,
                domainEvent.Price),
            cancellationToken);
    }
}

