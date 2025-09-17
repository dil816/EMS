using EMS.Common.Application.Messaging;
using EMS.Modules.Ticketing.Application.Abstractions.Payments;
using EMS.Modules.Ticketing.Domain.Payments;

namespace EMS.Modules.Ticketing.Application.Payments.RefundPayment;
internal sealed class PaymentPartiallyRefundedDomainEventHandler(IPaymentService paymentService)
    : DomainEventHandler<PaymentPartiallyRefundedDomainEvent>
{
    public override async Task Handle(
        PaymentPartiallyRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await paymentService.RefundAsync(domainEvent.TransactionId, domainEvent.RefundAmount);
    }
}
