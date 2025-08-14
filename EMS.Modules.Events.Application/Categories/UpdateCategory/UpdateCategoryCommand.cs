using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.Categories.UpdateCategory;
public sealed record UpdateCategoryCommand(Guid CategoryId, string Name) : ICommand;

