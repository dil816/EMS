using EMS.Common.Domain;
using MediatR;

namespace EMS.Common.Application.Messaging;
public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
