using EMS.Modules.Attendance.Infrastructure.Database;
using EMS.Modules.Events.Infrastructure.Database;
using EMS.Modules.Ticketing.Infrastructure.Database;
using EMS.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EMS.Api.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ApplyMigration<EventsDbContext>(scope);
        ApplyMigration<UsersDbContext>(scope);
        ApplyMigration<TicketingDbContext>(scope);
        ApplyMigration<AttendanceDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}
