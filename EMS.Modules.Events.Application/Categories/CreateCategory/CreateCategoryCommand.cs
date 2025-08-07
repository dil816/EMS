using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.Categories.CreateCategory;
public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;

