using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Orders.GetOrder;
using EMS.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace EMS.Modules.Ticketing.Application.Orders.CreateOrder;
internal sealed class SendOrderConfirmationDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result<OrderResponse> result = await sender.Send(new GetOrderQuery(notification.OrderId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(GetOrderQuery), result.Error);
        }

        // Send order confirmation notification.
    }
}
