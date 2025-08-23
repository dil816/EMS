using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Carts.RemoveItemFromCart;
public sealed record RemoveItemFromCartCommand(Guid CustomerId, Guid TicketTypeId) : ICommand;

