using System.Data.Common;
using EMS.Modules.Events.Application.Abstractions.Data;
using Npgsql;

namespace EMS.Modules.Events.Infrastructure.Data;
internal class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
