using EMS.Common.Domain;

namespace EMS.Modules.Attendance.Domain.Tickets;

public sealed class TicketCreatedDomainEvent(Guid ticketId, Guid eventId) : DomainEvent
{
    public Guid TicketId { get; init; } = ticketId;

    public Guid EventId { get; init; } = eventId;
}
