using EMS.Common.Domain;
using EMS.Modules.Ticketing.Domain.Customers;
using EMS.Modules.Ticketing.Domain.Orders;
using EMS.Modules.Ticketing.Domain.Payments;
using EMS.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EMS.Modules.Ticketing.UnitTests.Payments;
public class PaymentTests
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenPaymentIsCreated()
    {
        //Arrange
        var customer = Customer.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Name.LastName());

        var order = Order.Create(customer);

        //Act
        Result<Payment> result = Payment.Create(
            order,
            Guid.NewGuid(),
            BaseTest.Faker.Random.Decimal(),
            BaseTest.Faker.Random.String(3));

        //Assert
        PaymentCreatedDomainEvent domainEvent =
            BaseTest.AssertDomainEventWasPublished<PaymentCreatedDomainEvent>(result.Value);

        domainEvent.PaymentId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Refund_ShouldReturnFailure_WhenAlreadyRefunded()
    {
        //Arrange
        decimal amount = BaseTest.Faker.Random.Decimal();

        var customer = Customer.Create(
            Guid.NewGuid(),
            BaseTest.Faker.Internet.Email(),
            BaseTest.Faker.Name.FirstName(),
            BaseTest.Faker.Name.LastName());

        var order = Order.Create(customer);

        Result<Payment> paymentResult = Payment.Create(
            order,
            Guid.NewGuid(),
            amount,
            BaseTest.Faker.Random.String(3));

        Payment payment = paymentResult.Value;

        payment.Refund(amount);

        //Act
        Result result = payment.Refund(amount);

        //Assert
        result.Error.Should().Be(PaymentErrors.AlreadyRefunded);
    }
}
