using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Tickets.GetTicket;
using EMS.Modules.Ticketing.Application.Tickets.GetTicketForOrder;
using EMS.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace EMS.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
internal sealed class OrderTicketsIssuedDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderTicketsIssuedDomainEvent>
{
    public override async Task Handle(
        OrderTicketsIssuedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result<IReadOnlyCollection<TicketResponse>> result = await sender.Send(
            new GetTicketsForOrderQuery(domainEvent.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(GetTicketsForOrderQuery), result.Error);
        }

        // Send ticket confirmation notification.
    }
}

