using FluentValidation;

namespace EMS.Modules.Events.Application.Events.PublishEvent;
internal sealed class PublishEventCommandValidator : AbstractValidator<PublishEventCommand>
{
    public PublishEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
    }
}
