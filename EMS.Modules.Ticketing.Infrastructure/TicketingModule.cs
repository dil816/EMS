using EMS.Common.Application.EventBus;
using EMS.Common.Application.Messaging;
using EMS.Common.Infrastructure.Outbox;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Events.IntegrationEvents;
using EMS.Modules.Ticketing.Application.Abstractions.Authentication;
using EMS.Modules.Ticketing.Application.Abstractions.Data;
using EMS.Modules.Ticketing.Application.Abstractions.Payments;
using EMS.Modules.Ticketing.Application.Carts;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Domain.Events;
using EMS.Modules.Ticketing.Domain.Orders;
using EMS.Modules.Ticketing.Domain.Payments;
using EMS.Modules.Ticketing.Domain.Tickets;
using EMS.Modules.Ticketing.Infrastructure.Authentication;
using EMS.Modules.Ticketing.Infrastructure.Customers;
using EMS.Modules.Ticketing.Infrastructure.Database;
using EMS.Modules.Ticketing.Infrastructure.Events;
using EMS.Modules.Ticketing.Infrastructure.Inbox;
using EMS.Modules.Ticketing.Infrastructure.Orders;
using EMS.Modules.Ticketing.Infrastructure.Outbox;
using EMS.Modules.Ticketing.Infrastructure.Payments;
using EMS.Modules.Ticketing.Infrastructure.Tickets;
using EMS.Modules.Users.IntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EMS.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services.AddIntegrationEventHandlers();

        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserRegisteredIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserProfileUpdatedIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<EventPublishedIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<TicketTypePriceChangedIntegrationEvent>>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicketingDbContext>((sp, options) =>
           options
               .UseNpgsql(
                   configuration.GetConnectionString("Database"),
                   npgSqlOptions => npgSqlOptions
                       .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing))
               .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
               .UseSnakeCaseNamingConvention());

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());
        services.AddSingleton<CartService>();
        services.AddSingleton<IPaymentService, PaymentService>();

        services.AddScoped<ICustomerContext, CustomerContext>();

        services.Configure<OutboxOptions>(configuration.GetSection("Ticketing:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Ticketing:Inbox"));

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
