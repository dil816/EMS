using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Events.CreateEvent;
using EMS.Modules.Events.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EMS.Modules.Attendance.Presentation.Events;
public sealed class EventPublishedIntegrationEventConsumer(ISender sender)
    : IConsumer<EventPublishedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<EventPublishedIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new CreateEventCommand(
                context.Message.EventId,
                context.Message.Title,
                context.Message.Description,
                context.Message.Location,
                context.Message.StartsAtUtc,
                context.Message.EndsAtUtc),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateEventCommand), result.Error);
        }
    }
}
