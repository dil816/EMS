using EMS.Common.Application.Messaging;

namespace EMS.Modules.Attendance.Application.Events.EventStatistics.GetEventStatistics;
public sealed record GetEventStatisticsQuery(Guid EventId) : IQuery<EventStatisticsResponse>;

