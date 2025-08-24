using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Tickets.GetTicket;
public sealed record GetTicketQuery(Guid TicketId) : IQuery<TicketResponse>;
