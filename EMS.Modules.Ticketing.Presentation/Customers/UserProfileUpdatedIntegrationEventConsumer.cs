using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Customers.UpdateCustomer;
using EMS.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EMS.Modules.Ticketing.Presentation.Customers;
public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserProfileUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserProfileUpdatedIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new UpdateCustomerCommand(
                context.Message.UserId,
                context.Message.FirstName,
                context.Message.LastName),
            context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(UpdateCustomerCommand), result.Error);
        }
    }
}
