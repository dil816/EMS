namespace EMS.Modules.Ticketing.Application.Cart;
public sealed class Cart
{
    public Guid CustomerId { get; init; }

    public List<CartItem> Items { get; init; } = [];

    internal static Cart CreateDefault(Guid customerId) => new() { CustomerId = customerId };
}
