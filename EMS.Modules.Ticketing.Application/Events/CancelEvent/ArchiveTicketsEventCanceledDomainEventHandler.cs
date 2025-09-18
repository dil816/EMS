using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Tickets.ArchiveTicketsForEvent;
using EMS.Modules.Ticketing.Domain.Events;
using MediatR;

namespace EMS.Modules.Ticketing.Application.Events.CancelEvent;
internal sealed class ArchiveTicketsEventCanceledDomainEventHandler(ISender sender)
    : DomainEventHandler<EventCanceledDomainEvent>
{
    public override async Task Handle(
        EventCanceledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new ArchiveTicketsForEventCommand(domainEvent.EventId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(ArchiveTicketsForEventCommand), result.Error);
        }
    }
}
