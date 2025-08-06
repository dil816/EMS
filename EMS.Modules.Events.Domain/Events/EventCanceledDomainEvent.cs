using EMS.Modules.Events.Domain.Abstractions;

namespace EMS.Modules.Events.Domain.Events;
internal class EventCanceledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
