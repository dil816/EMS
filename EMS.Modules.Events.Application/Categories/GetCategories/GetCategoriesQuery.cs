using EMS.Modules.Events.Application.Abstractions.Messaging;
using EMS.Modules.Events.Application.Categories.GetCategory;

namespace EMS.Modules.Events.Application.Categories.GetCategories;
public sealed record GetCategoriesQuery : IQuery<IReadOnlyCollection<CategoryResponse>>;
