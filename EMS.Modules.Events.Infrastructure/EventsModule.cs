﻿using EMS.Common.Application.EventBus;
using EMS.Common.Application.Messaging;
using EMS.Common.Infrastructure.Outbox;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Events.Application.Abstractions.Data;
using EMS.Modules.Events.Domain.Categories;
using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.Domain.TicketTypes;
using EMS.Modules.Events.Infrastructure.Categories;
using EMS.Modules.Events.Infrastructure.Database;
using EMS.Modules.Events.Infrastructure.Events;
using EMS.Modules.Events.Infrastructure.Inbox;
using EMS.Modules.Events.Infrastructure.Outbox;
using EMS.Modules.Events.Infrastructure.PublicApi;
using EMS.Modules.Events.Infrastructure.TicketTypes;
using EMS.Modules.Events.Presentation.Events.CancelEventSaga;
using EMS.Modules.Events.PublicApi;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EMS.Modules.Events.Infrastructure;
public static class EventsModule
{
    public static IServiceCollection AddEventsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services.AddIntegrationEventHandlers();

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        services.AddInfrastructure(configuration);

        return services;
    }

    public static Action<IRegistrationConfigurator> ConfigureConsumers(string redisConnectionString)
    {
        return registrationConfigurator => registrationConfigurator
            .AddSagaStateMachine<CancelEventSaga, CancelEventState>()
            .RedisRepository(redisConnectionString);
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<EventsDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    databaseConnectionString,
                    npgSqlOptions => npgSqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Events))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEventsApi, EventsApi>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Events:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
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

    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }
}
