using EMS.Modules.Events.Application.Abstractions.Data;
using EMS.Modules.Events.Application.Abstractions.Messaging;
using EMS.Modules.Events.Domain.Abstractions;
using EMS.Modules.Events.Domain.Events;
using EMS.Modules.Events.Domain.TicketTypes;

namespace EMS.Modules.Events.Application.TicketTypes.CreateTicketType;
internal class CreateTicketTypeCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketTypeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure<Guid>(EventErrors.NotFound(request.EventId));
        }

        var ticketType = TicketType.Create(@event, request.Name, request.Price, request.Currency, request.Quantity);

        ticketTypeRepository.Insert(ticketType);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ticketType.Id;
    }
}
