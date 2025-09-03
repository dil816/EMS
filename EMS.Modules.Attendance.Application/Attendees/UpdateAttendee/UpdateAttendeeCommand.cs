using EMS.Common.Application.Messaging;

namespace EMS.Modules.Attendance.Application.Attendees.UpdateAttendee;
public sealed record UpdateAttendeeCommand(Guid AttendeeId, string FirstName, string LastName) : ICommand;
