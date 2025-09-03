using EMS.Common.Application.Messaging;

namespace EMS.Modules.Attendance.Application.Events.CreateEvent;
public sealed record CreateEventCommand(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc) : ICommand;
