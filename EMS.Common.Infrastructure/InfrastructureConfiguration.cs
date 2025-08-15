using EMS.Common.Application.Caching;
using EMS.Common.Application.Clock;
using EMS.Common.Application.Data;
using EMS.Common.Infrastructure.Caching;
using EMS.Common.Infrastructure.Clock;
using EMS.Common.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace EMS.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databaseConnectionString,
        string redisConnectionString)
    {
        NpgsqlDataSource npgSqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgSqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.TryAddSingleton(connectionMultiplexer);

        services.AddStackExchangeRedisCache(options =>
            options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer));

        services.TryAddSingleton<ICacheService, CacheService>();

        return services;
    }
}
