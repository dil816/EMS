using FluentAssertions;
using NetArchTest.Rules;


namespace EMS.Modules.Users.ArchitectureTests.Abstractions;
internal static class TestResultExtensions
{
    internal static void ShouldBeSuccessful(this TestResult testResult)
    {
        testResult.FailingTypes?.Should().BeEmpty();
    }
}
