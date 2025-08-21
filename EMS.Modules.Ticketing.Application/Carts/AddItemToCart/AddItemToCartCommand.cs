using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Carts.AddItemToCart;
public sealed record AddItemToCartCommand(Guid CustomerId, Guid TicketTypeId, decimal Quantity) : ICommand;

