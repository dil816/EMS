using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Tickets.ArchiveTicketsForEvent;
public sealed record ArchiveTicketsForEventCommand(Guid EventId) : ICommand;
