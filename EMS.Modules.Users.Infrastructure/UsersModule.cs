using EMS.Common.Infrastructure.Interceptors;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Users.Application.Abstractions.Data;
using EMS.Modules.Users.Domain.Users;
using EMS.Modules.Users.Infrastructure.Database;
using EMS.Modules.Users.Infrastructure.PublicApi;
using EMS.Modules.Users.Infrastructure.Users;
using EMS.Modules.Users.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Modules.Users.Infrastructure;
public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    NpgSqlOptionsExtensions => NpgSqlOptionsExtensions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>())
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

        services.AddScoped<IUsersApi, UsersApi>();
    }
}
