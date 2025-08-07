using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.TicketTypes.GetTicketType;
public sealed record GetTicketTypeQuery(Guid TicketTypeId) : IQuery<TicketTypeResponse>;
