using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
public sealed record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : ICommand;

