using EMS.Modules.Events.Application.Abstractions.Data;
using EMS.Modules.Events.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace EMS.Modules.Events.Infrastructure.Database;
public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Event> Events { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Events);
    }
}
