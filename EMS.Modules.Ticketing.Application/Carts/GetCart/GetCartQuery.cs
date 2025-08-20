using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Carts.GetCart;
public sealed record GetCartQuery(Guid CustomerId) : IQuery<Cart>;

