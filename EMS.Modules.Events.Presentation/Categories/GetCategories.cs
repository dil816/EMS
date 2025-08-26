using EMS.Common.Application.Caching;
using EMS.Common.Domain;
using EMS.Common.Presentation.ApiResults;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Events.Application.Categories.GetCategories;
using EMS.Modules.Events.Application.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EMS.Modules.Events.Presentation.Categories;

internal sealed class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, ICacheService cacheService) =>
        {
            IReadOnlyCollection<CategoryResponse> categoryResponses =
                await cacheService.GetAsync<IReadOnlyCollection<CategoryResponse>>("categories");

            if (categoryResponses is not null)
            {
                return Results.Ok(categoryResponses);
            }

            Result<IReadOnlyCollection<CategoryResponse>> result = await sender.Send(new GetCategoriesQuery());

            if (result.IsSuccess)
            {
                await cacheService.SetAsync("categories", result.Value);
            }

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Categories);
    }
}
