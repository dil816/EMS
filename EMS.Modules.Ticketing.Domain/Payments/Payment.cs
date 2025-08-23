using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Ticketing.Domain.Orders;

namespace EMS.Modules.Ticketing.Domain.Payments;
public sealed class Payment : Entity
{
    private Payment() { }

    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid TransactionId { get; private set; }

    public decimal Amount { get; private set; }

    public string Currency { get; private set; }

    public decimal? AmountRefunded { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? RefundedAtUtc { get; private set; }

    public static Payment Create(Order order, Guid transactionId, decimal amount, string currency)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            TransactionId = transactionId,
            Amount = amount,
            Currency = currency,
            CreatedAtUtc = DateTime.UtcNow
        };

        payment.Raise(new PaymentCreatedDomainEvent(payment.Id));

        return payment;
    }

    public Result Refund(decimal refundAmount)
    {
        if (AmountRefunded.HasValue && AmountRefunded == Amount)
        {
            return Result.Failure(PaymentErrors.AlreadyRefunded);
        }

        if (AmountRefunded + refundAmount > Amount)
        {
            return Result.Failure(PaymentErrors.NotEnoughFunds);
        }

        AmountRefunded += refundAmount;

        if (Amount == AmountRefunded)
        {
            Raise(new PaymentRefundedDomainEvent(Id, TransactionId, refundAmount));
        }
        else
        {
            Raise(new PaymentPartiallyRefundedDomainEvent(Id, TransactionId, refundAmount));
        }

        return Result.Success();
    }
}

public sealed class PaymentCreatedDomainEvent(Guid paymentId) : DomainEvent
{
    public Guid PaymentId { get; init; } = paymentId;
}

public static class PaymentErrors
{
    public static Error NotFound(Guid paymentId) =>
        Error.NotFound("Payments.NotFound", $"The payment with the identifier {paymentId} was not found");

    public static readonly Error AlreadyRefunded =
        Error.Problem("Payments.AlreadyRefunded", "The payment was already refunded");

    public static readonly Error NotEnoughFunds =
        Error.Problem("Payments.NotEnoughFunds", "There are not enough funds for a refund");
}

public sealed class PaymentPartiallyRefundedDomainEvent(Guid paymentId, Guid transactionId, decimal refundAmount)
    : DomainEvent
{
    public Guid PaymentId { get; init; } = paymentId;

    public Guid TransactionId { get; init; } = transactionId;

    public decimal RefundAmount { get; init; } = refundAmount;
}

public sealed class PaymentRefundedDomainEvent(Guid paymentId, Guid transactionId, decimal refundAmount)
    : DomainEvent
{
    public Guid PaymentId { get; init; } = paymentId;

    public Guid TransactionId { get; init; } = transactionId;

    public decimal RefundAmount { get; init; } = refundAmount;
}

public interface IPaymentRepository
{
    Task<Payment?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Payment>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default);

    void Insert(Payment payment);
}

