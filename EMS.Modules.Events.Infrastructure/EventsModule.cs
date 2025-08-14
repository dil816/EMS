using EMS.Modules.Events.Application.Abstractions.Data;
using EMS.Modules.Events.Domain.Categories;
using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.Domain.TicketTypes;
using EMS.Modules.Events.Infrastructure.Categories;
using EMS.Modules.Events.Infrastructure.Database;
using EMS.Modules.Events.Infrastructure.Events;
using EMS.Modules.Events.Infrastructure.TicketTypes;
using EMS.Modules.Events.Presentation.Categories;
using EMS.Modules.Events.Presentation.Events;
using EMS.Modules.Events.Presentation.TicketTypes;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Modules.Events.Infrastructure;
public static class EventsModule
{
    public static void MapEndPoints(IEndpointRouteBuilder app)
    {
        TicketTypeEndpoints.MapEndpoints(app);
        CategoryEndpoints.MapEndpoints(app);
        EventEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddEventsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<EventsDbContext>(options =>
            options
                .UseNpgsql(
                    databaseConnectionString,
                    npgSqlOptions => npgSqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Events))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors());

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
    }
}
