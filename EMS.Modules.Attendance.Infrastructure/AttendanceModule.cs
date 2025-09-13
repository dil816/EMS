using EMS.Common.Application.Messaging;
using EMS.Common.Infrastructure.Outbox;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Attendance.Application.Abstractions.Authentication;
using EMS.Modules.Attendance.Application.Abstractions.Data;
using EMS.Modules.Attendance.Domain.Attendees;
using EMS.Modules.Attendance.Domain.Events;
using EMS.Modules.Attendance.Domain.Tickets;
using EMS.Modules.Attendance.Infrastructure.Attendees;
using EMS.Modules.Attendance.Infrastructure.Authentication;
using EMS.Modules.Attendance.Infrastructure.Database;
using EMS.Modules.Attendance.Infrastructure.Events;
using EMS.Modules.Attendance.Infrastructure.Outbox;
using EMS.Modules.Attendance.Infrastructure.Tickets;
using EMS.Modules.Attendance.Presentation.Attendees;
using EMS.Modules.Attendance.Presentation.Events;
using EMS.Modules.Attendance.Presentation.Tickets;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EMS.Modules.Attendance.Infrastructure;

public static class AttendanceModule
{
    public static IServiceCollection AddAttendanceModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
        registrationConfigurator.AddConsumer<UserProfileUpdatedIntegrationEventConsumer>();
        registrationConfigurator.AddConsumer<EventPublishedIntegrationEventConsumer>();
        registrationConfigurator.AddConsumer<TicketIssuedIntegrationEventConsumer>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AttendanceDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgSqlOptions => npgSqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Attendance))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AttendanceDbContext>());

        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<IAttendanceContext, AttendanceContext>();

        services.Configure<OutboxOptions>(configuration.GetSection("Attendance:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();
    }

    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
}
