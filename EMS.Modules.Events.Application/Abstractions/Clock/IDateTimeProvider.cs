namespace EMS.Modules.Events.Application.Abstractions.Clock;
public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
