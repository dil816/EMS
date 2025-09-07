using EMS.Common.Application.EventBus;
using EMS.Common.Application.Messaging;
using EMS.Modules.Users.Domain.Users;
using EMS.Modules.Users.IntegrationEvents;

namespace EMS.Modules.Users.Application.Users.UpdateUser;
internal sealed class UserProfileUpdatedDomainEventHandler(IEventBus eventBus)
    : IDomainEventHandler<UserProfileUpdatedDomainEvent>
{
    public async Task Handle(UserProfileUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(
            new UserProfileUpdatedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.UserId,
                domainEvent.FirstName,
                domainEvent.LastName),
            cancellationToken);
    }
}
