using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Ticketing.UnitTests.Customers;
public class CustomerTests
{
    [Fact]
    public void Create_ShouldReturnValue_WhenCustomerIsCreated()
    {
        //Act
        Result<Customer> result = Customer.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Name.LastName());
        //Assert
        result.Value.Should().NotBeNull();
    }
}
