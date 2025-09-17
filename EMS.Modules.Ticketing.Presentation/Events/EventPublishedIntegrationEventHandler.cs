using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Events.IntegrationEvents;
using EMS.Modules.Ticketing.Application.Events.CreateEvent;
using MediatR;

namespace EMS.Modules.Ticketing.Presentation.Events;
public sealed class EventPublishedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public override async Task Handle(
        EventPublishedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateEventCommand(
                integrationEvent.EventId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Location,
                integrationEvent.StartsAtUtc,
                integrationEvent.EndsAtUtc,
                integrationEvent.TicketTypes
                .Select(t => new CreateEventCommand.TicketTypeRequest(
                       t.Id,
                       integrationEvent.EventId,
                       t.Name,
                       t.Price,
                       t.Currency,
                       t.Quantity))
                .ToList()),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateEventCommand), result.Error);
        }
    }
}
