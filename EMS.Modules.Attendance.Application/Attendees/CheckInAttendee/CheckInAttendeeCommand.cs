using EMS.Common.Application.Messaging;

namespace EMS.Modules.Attendance.Application.Attendees.CheckInAttendee;
public sealed record CheckInAttendeeCommand(Guid AttendeeId, Guid TicketId) : ICommand;

