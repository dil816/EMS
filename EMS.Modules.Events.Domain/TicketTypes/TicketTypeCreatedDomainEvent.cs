using EMS.Modules.Events.Domain.Abstractions;

namespace EMS.Modules.Events.Domain.TicketTypes;
public sealed class TicketTypeCreatedDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
}
