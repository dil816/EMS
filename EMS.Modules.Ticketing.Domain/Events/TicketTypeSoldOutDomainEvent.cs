﻿using EMS.Common.Domain;

namespace EMS.Modules.Ticketing.Domain.Events;

public sealed class TicketTypeSoldOutDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
}
