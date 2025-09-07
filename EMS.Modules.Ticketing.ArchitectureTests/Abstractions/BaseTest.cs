using System.Reflection;
using EMS.Modules.Ticketing.Domain.Orders;
using EMS.Modules.Ticketing.Infrastructure;

namespace EMS.Modules.Ticketing.ArchitectureTests.Abstractions;
internal static class BaseTest
{
    public static readonly Assembly ApplicationAssembly = typeof(Ticketing.Application.AssemblyReference).Assembly;

    public static readonly Assembly DomainAssembly = typeof(Order).Assembly;

    public static readonly Assembly InfrastructureAssembly = typeof(TicketingModule).Assembly;

    public static readonly Assembly PresentationAssembly = typeof(Ticketing.Presentation.AssemblyReference).Assembly;
}

