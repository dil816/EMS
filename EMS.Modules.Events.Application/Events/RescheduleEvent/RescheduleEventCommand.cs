using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.Events.RescheduleEvent;
public sealed record RescheduleEventCommand(Guid EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc) : ICommand;
