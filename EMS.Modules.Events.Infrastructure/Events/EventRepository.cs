using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.Infrastructure.Database;

namespace EMS.Modules.Events.Infrastructure.Events;
internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
