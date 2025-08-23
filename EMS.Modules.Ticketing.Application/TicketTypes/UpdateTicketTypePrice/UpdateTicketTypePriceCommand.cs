using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;
public sealed record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : ICommand;
