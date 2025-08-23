using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Orders.CreateOrder;
public sealed record CreateOrderCommand(Guid CustomerId) : ICommand;

