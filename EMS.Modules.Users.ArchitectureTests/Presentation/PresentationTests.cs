using EMS.Modules.Users.ArchitectureTests.Abstractions;
using MassTransit;
using NetArchTest.Rules;

namespace EMS.Modules.Users.ArchitectureTests.Presentation;
public class PresentationTests
{
    [Fact]
    public void IntegrationEventHandler_Should_BeSealed()
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
    public void IntegrationEventHandler_ShouldHave_NameEndingWith_DomainEventHandler()
    {
        Types.InAssembly(BaseTest.PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .HaveNameEndingWith("Consumer")
            .GetResult()
            .ShouldBeSuccessful();
    }
}
