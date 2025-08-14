using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.Events.CancelEvent;
public sealed record CancelEventCommand(Guid EventId) : ICommand;
