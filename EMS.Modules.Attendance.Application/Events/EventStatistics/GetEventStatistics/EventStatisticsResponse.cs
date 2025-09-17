namespace EMS.Modules.Attendance.Application.Events.EventStatistics.GetEventStatistics;

public sealed record EventStatisticsResponse(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int TicketsSold,
    int AttendeesCheckedIn,
    string[] DuplicateCheckInTickets,
    string[] InvalidCheckInTickets);

