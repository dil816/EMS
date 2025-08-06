namespace EMS.Modules.Events.Application.Events.GetEvent;
public sealed record class TicketTypeResponse(
    Guid TicketTypeId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);
