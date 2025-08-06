using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.Events.PublishEvent;
public sealed record PublishEventCommand(Guid EventId) : ICommand;

