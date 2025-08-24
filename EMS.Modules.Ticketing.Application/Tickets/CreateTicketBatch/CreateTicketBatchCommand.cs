using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
public sealed record CreateTicketBatchCommand(Guid OrderId) : ICommand;
