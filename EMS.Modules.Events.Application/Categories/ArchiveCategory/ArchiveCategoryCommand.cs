using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.Categories.ArchiveCategory;
public sealed record ArchiveCategoryCommand(Guid CategoryId) : ICommand;

