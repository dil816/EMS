using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using EMS.Common.Domain;
using EMS.Modules.Events.Application.Categories.GetCategories;
using EMS.Modules.Events.Application.Categories.GetCategory;
using EMS.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Events.IntegrationTests.Categories;
public class GetCategoriesTests : BaseIntegrationTest
{
    public GetCategoriesTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnEmptyCollection_WhenNoCategoriesExist()
    {
        // Arrange
        await CleanDatabaseAsync();

        var query = new GetCategoriesQuery();

        // Act
        Result<IReadOnlyCollection<CategoryResponse>> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnCategory_WhenCategoryExists()
    {
        // Arrange
        await CleanDatabaseAsync();

        await Sender.CreateCategoryAsync(Faker.Music.Genre());
        await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var query = new GetCategoriesQuery();

        // Act
        Result<IReadOnlyCollection<CategoryResponse>> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
}

