using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Orders.GetOrder;
using EMS.Modules.Ticketing.Domain.Orders;
using EMS.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Ticketing.Application.Orders.CreateOrder;
internal sealed class OrderCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
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

        await eventBus.PublishAsync(
            new OrderCreatedIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.TotalPrice,
                result.Value.CreatedAtUtc,
                result.Value.OrderItems.Select(oi => new OrderItemModel
                {
                    Id = oi.OrderItemId,
                    OrderId = result.Value.Id,
                    TicketTypeId = oi.TicketTypeId,
                    Price = oi.Price,
                    UnitPrice = oi.UnitPrice,
                    Currency = oi.Currency,
                    Quantity = oi.Quantity
                }).ToList()),
            cancellationToken);
    }
}

