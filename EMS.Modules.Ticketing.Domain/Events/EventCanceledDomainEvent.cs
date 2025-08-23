using EMS.Common.Domain;

namespace EMS.Modules.Ticketing.Domain.Events;

public sealed class EventCanceledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}
