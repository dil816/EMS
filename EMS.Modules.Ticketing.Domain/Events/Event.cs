using EMS.Common.Domain;

namespace EMS.Modules.Ticketing.Domain.Events;
public sealed class Event : Entity
{
    private Event()
    {

    }

    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Location { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime? EndsAtUtc { get; private set; }

    public bool Canceled { get; private set; }

    public static Event Create(
        Guid id,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        var @event = new Event
        {
            Id = id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc
        };

        return @event;
    }

    public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;

        Raise(new EventRescheduledDomainEvent(Id, StartsAtUtc, EndsAtUtc));
    }

    public void Cancel()
    {
        if (Canceled)
        {
            return;
        }

        Canceled = true;

        Raise(new EventCanceledDomainEvent(Id));
    }

    public void PaymentsRefunded()
    {
        Raise(new EventPaymentsRefundedDomainEvent(Id));
    }

    public void TicketsArchived()
    {
        Raise(new EventTicketsArchivedDomainEvent(Id));
    }
}


public sealed class EventCanceledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}

public sealed class EventPaymentsRefundedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}

public sealed class EventTicketsArchivedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}

public sealed class EventRescheduledDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc)
    : DomainEvent
{
    public Guid EventId { get; } = eventId;

    public DateTime StartsAtUtc { get; } = startsAtUtc;

    public DateTime? EndsAtUtc { get; } = endsAtUtc;
}

public static class EventErrors
{
    public static Error NotFound(Guid eventId) =>
        Error.NotFound("Events.NotFound", $"The event with the identifier {eventId} was not found");

    public static readonly Error StartDateInPast = Error.Problem(
        "Events.StartDateInPast",
        "The event start date is in the past");
}



public interface IEventRepository
{
    Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Event @event);
}


public sealed class TicketType : Entity
{
    private TicketType()
    {

    }

    public Guid Id { get; private set; }

    public Guid EventId { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    public decimal Quantity { get; private set; }

    public decimal AvailableQuantity { get; private set; }

    public static TicketType Create(
        Guid id,
        Guid eventId,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        var ticketType = new TicketType
        {
            Id = id,
            EventId = eventId,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity,
            AvailableQuantity = quantity
        };

        return ticketType;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }

    public Result UpdateQuantity(decimal quantity)
    {
        if (AvailableQuantity < quantity)
        {
            Result.Failure(TicketTypeErrors.NotEnoughQuantity(AvailableQuantity));
        }

        AvailableQuantity -= quantity;

        if (AvailableQuantity == 0)
        {
            Raise(new TicketTypeSoldOutDomainEvent(Id));
        }

        return Result.Success();
    }
}

public sealed class TicketTypeSoldOutDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
}

public interface ITicketTypeRepository
{
    Task<TicketType?> GetAsync(Guid Id, CancellationToken cancellationToken = default);

    Task<TicketType?> GetWithLockAsync(Guid Id, CancellationToken cancellationToken = default);

    void InsertRange(IEnumerable<TicketType> ticketTypes);
}
