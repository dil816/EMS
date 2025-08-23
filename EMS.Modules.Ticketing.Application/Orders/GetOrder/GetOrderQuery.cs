using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Orders.GetOrder;
public sealed record GetOrderQuery(Guid OrderId) : IQuery<OrderResponse>;
