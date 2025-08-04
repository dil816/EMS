using System.Data.Common;

namespace EMS.Modules.Events.Application.Abstractions.Data;
public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
