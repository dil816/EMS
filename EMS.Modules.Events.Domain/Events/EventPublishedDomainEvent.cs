using EMS.Common.Domain;

namespace EMS.Modules.Events.Domain.Events;
public sealed class EventPublishedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
