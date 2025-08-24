using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;
public sealed record RefundPaymentsForEventCommand(Guid EventId) : ICommand;
