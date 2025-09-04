using System.Reflection;
using EMS.Architecture.Tests.Abstractions;
using EMS.Modules.Users.Domain.Users;
using EMS.Modules.Users.Infrastructure;
using NetArchTest.Rules;

namespace EMS.Architecture.Tests.Layers;
public class ModuleTests : BaseTest
{
    [Fact]
    public void UserModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [EventsNamespace, TicketingNamespace, AttendanceNamespace];


        List<Assembly> usersAssemblies =
        [
            typeof(User).Assembly,
            Modules.Users.Application.AssemblyReference.Assembly,
            Modules.Users.Presentation.AssemblyReference.Assembly,
            typeof(UsersModule).Assembly
        ];

        Types.InAssemblies(usersAssemblies)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
}
