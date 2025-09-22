using Bogus;
using EMS.Common.Domain;
using EMS.Modules.Events.Application.Categories.UpdateCategory;
using EMS.Modules.Events.Domain.Categories;
using EMS.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Events.IntegrationTests.Categories;
public class UpdateCategoryTests : BaseIntegrationTest
{
    public UpdateCategoryTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public static readonly TheoryData<UpdateCategoryCommand> InvalidCommands = new()
    {
        new UpdateCategoryCommand(Guid.Empty, Faker.Music.Genre()),
        new UpdateCategoryCommand(Guid.NewGuid(), string.Empty)
    };

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnFailure_WhenCommandIsNotValid(UpdateCategoryCommand command)
    {
        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new UpdateCategoryCommand(Guid.NewGuid(), Faker.Music.Genre());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(CategoryErrors.NotFound(command.CategoryId));
    }

    [Fact]
    public async Task Should_UpdateCategory_WhenCategoryExists()
    {
        // Arrange
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var command = new UpdateCategoryCommand(categoryId, Faker.Music.Genre());

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
