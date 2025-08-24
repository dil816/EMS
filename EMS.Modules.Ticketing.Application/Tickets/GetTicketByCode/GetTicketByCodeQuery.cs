using EMS.Common.Application.Messaging;
using EMS.Modules.Ticketing.Application.Tickets.GetTicket;

namespace EMS.Modules.Ticketing.Application.Tickets.GetTicketByCode;
public sealed record GetTicketByCodeQuery(string Code) : IQuery<TicketResponse>;

