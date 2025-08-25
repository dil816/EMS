using EMS.Modules.Ticketing.Domain.Orders;
using EMS.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EMS.Modules.Ticketing.Infrastructure.Orders;
internal sealed class OrderRepository(TicketingDbContext context) : IOrderRepository
{
    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public void Insert(Order order)
    {
        context.Orders.Add(order);
    }
}
