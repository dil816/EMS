using EMS.Common.Domain;
using EMS.Modules.Events.Application.TicketTypes.GetTicketType;
using EMS.Modules.Events.PublicApi;
using MediatR;
using TicketTypeResponse = EMS.Modules.Events.PublicApi.TicketTypeResponse;

namespace EMS.Modules.Events.Infrastructure.PublicApi;
internal sealed class EventsApi(ISender sender) : IEventsApi
{
    public async Task<TicketTypeResponse?> GetTicketTypeAsync(
        Guid ticketTypeId,
        CancellationToken cancellationToken = default)
    {
        Result<Application.TicketTypes.GetTicketType.TicketTypeResponse> result =
            await sender.Send(new GetTicketTypeQuery(ticketTypeId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        return new TicketTypeResponse(
            result.Value.Id,
            result.Value.EventId,
            result.Value.Name,
            result.Value.Price,
            result.Value.Currency,
            result.Value.Quantity);
    }
}
