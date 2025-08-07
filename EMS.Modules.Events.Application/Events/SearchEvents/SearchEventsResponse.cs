using EMS.Modules.Events.Application.Events.GetEvents;

namespace EMS.Modules.Events.Application.Events.SearchEvents;
public sealed record SearchEventsResponse(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<EventResponse> Events);

