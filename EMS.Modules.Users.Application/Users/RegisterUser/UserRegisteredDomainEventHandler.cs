using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.PublicApi;
using EMS.Modules.Users.Application.Users.GetUser;
using EMS.Modules.Users.Domain.Users;
using MediatR;

namespace EMS.Modules.Users.Application.Users.RegisterUser;
internal sealed class UserRegisteredDomainEventHandler(ISender sender, ITicketingApi ticketingApi)
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(GetUserQuery), result.Error);
        }

        await ticketingApi.CreateCustomerAsync(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName,
            cancellationToken);
    }
}
