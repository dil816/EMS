using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Events.IntegrationEvents;
using EMS.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;
using MassTransit;
using MediatR;

namespace EMS.Modules.Ticketing.Presentation.TicketTypes;
public sealed class TicketTypePriceChangedIntegrationEventConsumer(ISender sender)
    : IConsumer<TicketTypePriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<TicketTypePriceChangedIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new UpdateTicketTypePriceCommand(context.Message.TicketTypeId, context.Message.Price),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(UpdateTicketTypePriceCommand), result.Error);
        }
    }
}
