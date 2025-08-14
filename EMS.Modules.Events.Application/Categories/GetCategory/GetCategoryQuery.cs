using EMS.Common.Application.Messaging;

namespace EMS.Modules.Events.Application.Categories.GetCategory;
public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryResponse>;
