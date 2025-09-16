using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Customers.CreateCustomer;
using EMS.Modules.Users.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Ticketing.Presentation.Customers;
public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(
        UserRegisteredIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
             new CreateCustomerCommand(
                 integrationEvent.UserId,
                 integrationEvent.Email,
                 integrationEvent.FirstName,
                 integrationEvent.LastName),
             cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
