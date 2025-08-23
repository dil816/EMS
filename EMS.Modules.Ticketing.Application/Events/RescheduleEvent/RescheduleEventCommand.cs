using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Events.RescheduleEvent;
public sealed record RescheduleEventCommand(Guid EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc) : ICommand;

