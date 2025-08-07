using EMS.Modules.Events.Application.Abstractions.Messaging;

namespace EMS.Modules.Events.Application.Categories.ArchiveCategory;
public sealed record ArchiveCategoryCommand(Guid CategoryId) : ICommand;

