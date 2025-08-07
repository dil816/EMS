using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.TicketTypes.CreateTicketType;
public sealed record CreateTicketTypeCommand(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity) : ICommand<Guid>;
