using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Customers.UpdateCustomer;
using EMS.Modules.Users.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Ticketing.Presentation.Customers;
public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(UserProfileUpdatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(UpdateCustomerCommand), result.Error);
        }
    }
}
