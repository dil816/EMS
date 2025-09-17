using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
using EMS.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace EMS.Modules.Ticketing.Application.Orders.CreateOrder;
internal sealed class CreateTicketsDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new CreateTicketBatchCommand(notification.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateTicketBatchCommand), result.Error);
        }
    }
}

