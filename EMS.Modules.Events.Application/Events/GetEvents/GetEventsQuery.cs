using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.Events.GetEvents;
public sealed record GetEventsQuery : IQuery<IReadOnlyCollection<EventResponse>>;
