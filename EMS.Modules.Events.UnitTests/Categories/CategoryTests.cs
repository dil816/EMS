using EMS.Common.Domain;
using EMS.Modules.Events.Domain.Categories;
using EMS.Modules.Events.UnitTests.Abstraction;
using FluentAssertions;

namespace EMS.Modules.Events.UnitTests.Categories;
public class CategoryTests
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenCategoryIsCreated()
    {
        //Act
        Result<Category> result = Category.Create(BaseTest.Faker.Music.Genre());

        //Assert
        CategoryCreatedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(result.Value);

        domainEvent.CategoryId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenCategoryIsArchived()
    {
        //Arrange
        Result<Category> result = Category.Create(BaseTest.Faker.Music.Genre());

        Category category = result.Value;

        //Act
        category.Archive();

        //Assert
        CategoryArchivedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<CategoryArchivedDomainEvent>(category);

        domainEvent.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public void ChangeName_ShouldRaiseDomainEvent_WhenCategoryNameIsChanged()
    {
        //Arrange
        Result<Category> result = Category.Create(BaseTest.Faker.Music.Genre());
        Category category = result.Value;
        category.ClearDomainEvents();

        string newName = BaseTest.Faker.Music.Genre();

        //Act
        category.ChangeName(newName);

        //Assert
        CategoryNameChangedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<CategoryNameChangedDomainEvent>(category);

        domainEvent.CategoryId.Should().Be(category.Id);
    }
}


