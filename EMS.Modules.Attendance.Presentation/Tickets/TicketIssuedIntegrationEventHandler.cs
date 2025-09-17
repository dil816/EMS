using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Attendance.Application.Tickets.CreateTicket;
using EMS.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Attendance.Presentation.Tickets;
public sealed class TicketIssuedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<TicketIssuedIntegrationEvent>
{
    public override async Task Handle(TicketIssuedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateTicketCommand(
                integrationEvent.TicketId,
                integrationEvent.CustomerId,
                integrationEvent.EventId,
                integrationEvent.Code),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateTicketCommand), result.Error);
        }
    }
}
