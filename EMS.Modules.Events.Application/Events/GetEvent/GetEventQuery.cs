using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.Events.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventResponse>;


