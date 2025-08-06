using EMS.Modules.Events.Domain.Abstractions;

namespace EMS.Modules.Events.Domain.Events;
public sealed class EventRescheduledDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
    public DateTime StartsAtUtc { get; init; } = startsAtUtc;
    public DateTime? EndsAtUtc { get; init; } = endsAtUtc;
}
