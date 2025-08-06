namespace EMS.Modules.Events.Application.Abstractions.Clock;
internal interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
