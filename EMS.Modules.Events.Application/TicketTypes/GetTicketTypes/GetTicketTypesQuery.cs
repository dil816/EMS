using EMS.Modules.Events.Application.Abstractions.Messaging;
using EMS.Modules.Events.Application.TicketTypes.GetTicketType;

namespace EMS.Modules.Events.Application.TicketTypes.GetTicketTypes;
public sealed record GetTicketTypesQuery(Guid EventId) : IQuery<IReadOnlyCollection<TicketTypeResponse>>;

