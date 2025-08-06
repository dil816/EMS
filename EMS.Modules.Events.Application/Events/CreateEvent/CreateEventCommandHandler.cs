using EMS.Modules.Events.Application.Abstractions.Clock;
using EMS.Modules.Events.Application.Abstractions.Data;
using EMS.Modules.Events.Application.Abstractions.Messaging;
using EMS.Modules.Events.Domain.Abstractions;
using EMS.Modules.Events.Domain.Events;
using MediatR;

namespace EMS.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return Result.Failure<Guid>(EventErrors.StartDateInPast);
        }

        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<Guid>(CategoryErrors.NotFound(request.CategoryId));
        }

        var @event = new Event
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartsAtUtc = request.StartsAtUtc,
            EndsAtUtc = request.EndsAtUtc
        };
        eventRepository.Insert(@event);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return @event.Id;
    }
}
