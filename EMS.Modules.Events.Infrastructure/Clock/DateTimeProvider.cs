using EMS.Modules.Events.Application.Abstractions.Clock;

namespace EMS.Modules.Events.Infrastructure.Clock;
internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
