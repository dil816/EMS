using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Orders.GetOrders;
public sealed record GetOrdersQuery(Guid CustomerId) : IQuery<IReadOnlyCollection<OrderResponse>>;

