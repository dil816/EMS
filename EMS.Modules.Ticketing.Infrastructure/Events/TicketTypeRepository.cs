using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EMS.Modules.Ticketing.Infrastructure.Events;
internal class TicketTypeRepository(TicketingDbContext context) : ITicketTypeRepository
{
    public async Task<TicketType?> GetAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await context.TicketTypes.SingleOrDefaultAsync(t => t.Id == Id, cancellationToken);
    }

    public async Task<TicketType?> GetWithLockAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await context
            .TicketTypes
            .FromSql(
                $"""
                SELECT id, event_id, name, price, currency, quantity, available_quantity
                FROM ticketing.ticket_types
                WHERE id = {Id}
                FOR UPDATE NOWAIT
                """)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void InsertRange(IEnumerable<TicketType> ticketTypes)
    {
        context.TicketTypes.AddRange(ticketTypes);
    }
}
