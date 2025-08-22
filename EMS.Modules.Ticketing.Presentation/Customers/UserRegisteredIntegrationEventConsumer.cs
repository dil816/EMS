using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Customers.CreateCustomer;
using EMS.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EMS.Modules.Ticketing.Presentation.Customers;
public sealed class UserRegisteredIntegrationEventConsumer(ISender sender) : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        Result result = await sender.Send(
             new CreateCustomerCommand(
                 context.Message.UserId,
                 context.Message.Email,
                 context.Message.FirstName,
                 context.Message.LastName));

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
