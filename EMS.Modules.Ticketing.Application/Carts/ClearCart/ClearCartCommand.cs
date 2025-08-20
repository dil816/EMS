using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Carts.ClearCart;
public sealed record ClearCartCommand(Guid CustomerId) : ICommand;

