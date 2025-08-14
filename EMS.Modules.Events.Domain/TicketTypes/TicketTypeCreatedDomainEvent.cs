using EMS.Common.Domain;

namespace EMS.Modules.Events.Domain.TicketTypes;
public sealed class TicketTypeCreatedDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
}
