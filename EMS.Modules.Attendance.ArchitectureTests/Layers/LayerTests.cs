using EMS.Modules.Attendance.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace EMS.Modules.Attendance.ArchitectureTests.Layers;
public class LayerTests
{
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        Types.InAssembly(BaseTest.DomainAssembly)
            .Should()
            .NotHaveDependencyOn(BaseTest.ApplicationAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        Types
            .InAssembly(BaseTest.DomainAssembly)
            .Should()
            .NotHaveDependencyOn(BaseTest.InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        Types.InAssembly(BaseTest.ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(BaseTest.InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        Types.InAssembly(BaseTest.ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(BaseTest.PresentationAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void PresentationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        Types.InAssembly(BaseTest.PresentationAssembly)
            .Should()
            .NotHaveDependencyOn(BaseTest.InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }
}

