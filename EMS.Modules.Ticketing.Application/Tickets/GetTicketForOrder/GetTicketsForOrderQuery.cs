using EMS.Common.Application.Messaging;
using EMS.Modules.Ticketing.Application.Tickets.GetTicket;

namespace EMS.Modules.Ticketing.Application.Tickets.GetTicketForOrder;
public sealed record GetTicketsForOrderQuery(Guid OrderId) : IQuery<IReadOnlyCollection<TicketResponse>>;
