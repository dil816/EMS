using EMS.Modules.Events.Domain.Abstractions;

namespace EMS.Modules.Events.Domain.Categories;
public sealed class CategoryCreatedDomainEvent(Guid CategoryId) : DomainEvent
{
    public Guid CategoryId { get; init; } = CategoryId;
}
