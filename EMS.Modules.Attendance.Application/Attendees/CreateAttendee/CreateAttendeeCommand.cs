using EMS.Common.Application.Messaging;

namespace EMS.Modules.Attendance.Application.Attendees.CreateAttendee;
public sealed record CreateAttendeeCommand(Guid AttendeeId, string Email, string FirstName, string LastName)
    : ICommand;

