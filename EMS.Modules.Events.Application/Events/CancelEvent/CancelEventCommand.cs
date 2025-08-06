using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.Events.CancelEvent;
internal sealed record CancelEventCommand(Guid EventId) : ICommand;
