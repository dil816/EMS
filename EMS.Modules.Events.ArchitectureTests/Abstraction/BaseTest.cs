using System.Reflection;
using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.Infrastructure;

namespace EMS.Modules.Events.ArchitectureTests.Abstraction;
internal static class BaseTest
{
    public static readonly Assembly ApplicationAssembly = typeof(Events.Application.AssemblyReference).Assembly;

    public static readonly Assembly DomainAssembly = typeof(Event).Assembly;

    public static readonly Assembly InfrastructureAssembly = typeof(EventsModule).Assembly;

    public static readonly Assembly PresentationAssembly = typeof(Events.Presentation.AssemblyReference).Assembly;
}
