using EMS.Common.Application.Messaging;
using EMS.Modules.Events.Domain.Events;

namespace EMS.Modules.Events.Application.Events.RescheduleEvent;
internal sealed class EventRescheduledDomainEventHandler
    : DomainEventHandler<EventRescheduledDomainEvent>
{
    public override Task Handle(EventRescheduledDomainEvent notification, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
