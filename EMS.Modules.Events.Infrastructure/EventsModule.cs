using EMS.Modules.Events.Api.Database;
using EMS.Modules.Events.Api.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Modules.Events.Api;
public static class EventsModule
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndpoint(app);
        GetEvent.MapEndpoint(app);
    }

    public static IServiceCollection AddEventsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<EventsDbContext>(options =>
            options
                .UseNpgsql(
                    databaseConnectionString,
                    npgSqlOptions => npgSqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Events))
                .UseSnakeCaseNamingConvention());

        return services;
    }
}
