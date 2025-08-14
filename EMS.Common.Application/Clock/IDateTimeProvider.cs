namespace EMS.Common.Application.Clock;
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
