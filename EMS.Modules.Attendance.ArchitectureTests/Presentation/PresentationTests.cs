using EMS.Modules.Attendance.ArchitectureTests.Abstractions;
using MassTransit;
using NetArchTest.Rules;

namespace EMS.Modules.Attendance.ArchitectureTests.Presentation;
public class PresentationTests
{
    [Fact]
    public void IntegrationEventConsumer_Should_BeSealed()
    {
        Types.InAssembly(BaseTest.PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEventConsumer_ShouldHave_NameEndingWith_IntegrationEventConsumer()
    {
        Types.InAssembly(BaseTest.PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .HaveNameEndingWith("IntegrationEventConsumer")
            .GetResult()
            .ShouldBeSuccessful();
    }
}
