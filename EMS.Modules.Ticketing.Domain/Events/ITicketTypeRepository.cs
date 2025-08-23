namespace EMS.Modules.Ticketing.Domain.Events;

public interface ITicketTypeRepository
{
    Task<TicketType?> GetAsync(Guid Id, CancellationToken cancellationToken = default);

    Task<TicketType?> GetWithLockAsync(Guid Id, CancellationToken cancellationToken = default);

    void InsertRange(IEnumerable<TicketType> ticketTypes);
}
