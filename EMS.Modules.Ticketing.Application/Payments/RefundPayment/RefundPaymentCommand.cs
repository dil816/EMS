using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Payments.RefundPayment;
public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;
