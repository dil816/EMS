using EMS.Common.Application.Messaging;

namespace EMS.Modules.Attendance.Application.Tickets.CreateTicket;
public sealed record CreateTicketCommand(Guid TicketId, Guid AttendeeId, Guid EventId, string Code) : ICommand;

