using System.Reflection;
using EMS.Modules.Users.Domain.Users;
using EMS.Modules.Users.Infrastructure;

namespace EMS.Modules.Users.ArchitectureTests.Abstractions;
internal static class BaseTest
{
    public static readonly Assembly ApplicationAssembly = typeof(Users.Application.AssemblyReference).Assembly;

    public static readonly Assembly DomainAssembly = typeof(User).Assembly;

    public static readonly Assembly InfrastructureAssembly = typeof(UsersModule).Assembly;

    public static readonly Assembly PresentationAssembly = typeof(Users.Presentation.AssemblyReference).Assembly;
}
