using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Carts;
using EMS.Modules.Ticketing.Application.Orders.CreateOrder;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Ticketing.IntegrationTests.Orders;
public class CreateOrderTests : BaseIntegrationTest
{
    public CreateOrderTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        //Arrange
        var command = new CreateOrderCommand(Guid.NewGuid());

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCartIsEmpty()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new CreateOrderCommand(customerId);

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CartErrors.Empty);
    }
}

