using MediatR;

namespace EMS.Modules.Events.Application.Events.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IRequest<EventResponse?>;


