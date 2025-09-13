﻿using EMS.Common.Application.EventBus;
using EMS.Common.Application.Exceptions;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Users.Application.Users.GetUser;
using EMS.Modules.Users.Domain.Users;
using EMS.Modules.Users.IntegrationEvents;
using MediatR;

namespace EMS.Modules.Users.Application.Users.RegisterUser;
internal sealed class UserRegisteredDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        Result<UserResponse> result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EmsException(nameof(GetUserQuery), result.Error);
        }

        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.Email,
                result.Value.FirstName,
                result.Value.LastName),
            cancellationToken);
    }
}
