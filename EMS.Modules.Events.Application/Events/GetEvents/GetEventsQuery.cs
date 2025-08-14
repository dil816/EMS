using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.Events.GetEvents;
public sealed record GetEventsQuery : IQuery<IReadOnlyCollection<EventResponse>>;
