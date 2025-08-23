using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Domain.Events;

namespace EMS.Modules.Ticketing.Domain.Orders;

public sealed class Order : Entity
{
    private readonly List<OrderItem> _orderItems = [];

    private Order()
    {
    }

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice { get; private set; }

    public string Currency { get; private set; }

    public bool TicketsIssued { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.ToList();

    public static Order Create(Customer customer)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            Status = OrderStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };

        order.Raise(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void AddItem(TicketType ticketType, decimal quantity, decimal price, string currency)
    {
        var orderItem = OrderItem.Create(Id, ticketType.Id, quantity, price, currency);

        _orderItems.Add(orderItem);

        TotalPrice = _orderItems.Sum(o => o.Price);
        Currency = currency;
    }

    public Result IssueTickets()
    {
        if (TicketsIssued)
        {
            return Result.Failure(OrderErrors.TicketsAlreadyIssues);
        }

        TicketsIssued = true;

        Raise(new OrderTicketsIssuedDomainEvent(Id));

        return Result.Success();
    }
}

public sealed class OrderCreatedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}

public sealed class OrderTicketsIssuedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}

public interface IOrderRepository
{
    Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Order order);
}

public static class OrderErrors
{
    public static Error NotFound(Guid orderId) =>
        Error.NotFound("Orders.NotFound", $"The order with the identifier {orderId} was not found");


    public static readonly Error TicketsAlreadyIssues = Error.Problem(
        "Order.TicketsAlreadyIssued",
        "The tickets for this order were already issued");
}

public enum OrderStatus
{
    Pending = 0,
    Paid = 1,
    Refunded = 2,
    Canceled = 3
}

public sealed class OrderItem
{
    private OrderItem()
    {
    }

    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid TicketTypeId { get; private set; }

    public decimal Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    internal static OrderItem Create(Guid orderId, Guid ticketTypeId, decimal quantity, decimal unitPrice, string currency)
    {
        var orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            TicketTypeId = ticketTypeId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Price = quantity * unitPrice,
            Currency = currency
        };

        return orderItem;
    }

}


