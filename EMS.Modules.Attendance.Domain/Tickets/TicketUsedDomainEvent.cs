using EMS.Common.Domain;

namespace EMS.Modules.Attendance.Domain.Tickets;

public sealed class TicketUsedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId { get; init; } = ticketId;
}
