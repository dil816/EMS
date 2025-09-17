using System.Data.Common;
using Dapper;
using EMS.Common.Application.Data;
using EMS.Common.Application.Messaging;
using EMS.Modules.Attendance.Domain.Attendees;

namespace EMS.Modules.Attendance.Application.Events.EventStatistics.Projections;
internal sealed class InvalidCheckInAttemptedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<InvalidCheckInAttemptedDomainEvent>
{
    public override async Task Handle(
        InvalidCheckInAttemptedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            UPDATE attendance.event_statistics es
            SET invalid_check_in_tickets = array_append(invalid_check_in_tickets, @TicketCode)
            WHERE es.event_id = @EventId
            """;

        await connection.ExecuteAsync(sql, domainEvent);
    }
}
