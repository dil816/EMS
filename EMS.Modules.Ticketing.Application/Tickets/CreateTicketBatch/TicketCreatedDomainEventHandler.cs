using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Tickets.GetTicket;
using EMS.Modules.Ticketing.Domain.Tickets;
using EMS.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
internal sealed class TicketCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result<TicketResponse> result = await sender.Send(
            new GetTicketQuery(domainEvent.TicketId),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(GetTicketQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new TicketIssuedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.EventId,
                result.Value.Code),
            cancellationToken);
    }
}

