using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.Categories.CreateCategory;
public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;

