using System.Reflection;
using EMS.Common.Domain;
using EMS.Modules.Users.ArchitectureTests.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace EMS.Modules.Users.ArchitectureTests.Domain;
public class DomainTests
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(BaseTest.DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        Types.InAssembly(BaseTest.DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(BaseTest.DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (Type entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.NonPublic |
                                                                        BindingFlags.Instance);

            if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(BaseTest.DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (Type entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.Public |
                                                                        BindingFlags.Instance);

            if (constructors.Any())
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
}
