using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Events.CancelEvent;
public sealed record CancelEventCommand(Guid EventId) : ICommand;
