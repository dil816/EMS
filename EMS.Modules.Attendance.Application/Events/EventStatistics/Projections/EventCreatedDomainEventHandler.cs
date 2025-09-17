using System.Data.Common;
using Dapper;
using EMS.Common.Application.Data;
using EMS.Common.Application.Messaging;
using EMS.Modules.Attendance.Domain.Events;

namespace EMS.Modules.Attendance.Application.Events.EventStatistics.Projections;
internal sealed class EventCreatedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<EventCreatedDomainEvent>
{
    public override async Task Handle(
        EventCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            INSERT INTO attendance.event_statistics(
                event_id,
                title,
                description,
                location,
                starts_at_utc,
                ends_at_utc,
                tickets_sold,
                attendees_checked_in,
                duplicate_check_in_tickets,
                invalid_check_in_tickets)
            VALUES (
                @EventId,
                @Title,
                @Description,
                @Location,
                @StartsAtUtc,
                @EndsAtUtc,
                @TicketsSold,
                @AttendeesCheckedIn,
                @DuplicateCheckInTickets,
                @InvalidCheckInTickets)
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                domainEvent.EventId,
                domainEvent.Title,
                domainEvent.Description,
                domainEvent.Location,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc,
                TicketsSold = 0,
                AttendeesCheckedIn = 0,
                DuplicateCheckInTickets = Array.Empty<string>(),
                InvalidCheckInTickets = Array.Empty<string>()
            });
    }
}
