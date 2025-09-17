using EMS.Common.Application.EventBus;
using EMS.Common.Application.Messaging;
using EMS.Modules.Ticketing.Domain.Tickets;
using EMS.Modules.Ticketing.IntegrationEvents;

namespace EMS.Modules.Ticketing.Application.Tickets.ArchiveTicket;
internal sealed class TicketArchivedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<TicketArchivedDomainEvent>
{
    public override async Task Handle(
        TicketArchivedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new TicketArchivedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.TicketId,
                domainEvent.Code),
            cancellationToken);
    }
}
