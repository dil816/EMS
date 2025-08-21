using EMS.Common.Infrastructure.Interceptors;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Ticketing.Application.Abstractions;
using EMS.Modules.Ticketing.Application.Carts;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Infrastructure.Customers;
using EMS.Modules.Ticketing.Infrastructure.Database;
using EMS.Modules.Ticketing.Infrastructure.PublicApi;
using EMS.Modules.Ticketing.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicketingDbContext>((sp, options) =>
           options
               .UseNpgsql(
                   configuration.GetConnectionString("Database"),
                   npgSqlOptions => npgSqlOptions
                       .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing))
               .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>())
               .UseSnakeCaseNamingConvention());

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());

        services.AddSingleton<CartService>();

        services.AddScoped<ITicketingApi, TicketingApi>();
    }
}
