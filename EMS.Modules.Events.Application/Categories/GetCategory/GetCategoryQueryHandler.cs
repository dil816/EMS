using System.Data.Common;
using Dapper;
using EMS.Common.Application.Data;
using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Events.Domain.Categories;

namespace EMS.Modules.Events.Application.Categories.GetCategory;
internal sealed class GetCategoryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCategoryQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
              SELECT
                id AS {nameof(CategoryResponse.Id)},
                name AS {nameof(CategoryResponse.Name)},
                is_archived AS {nameof(CategoryResponse.IsArchived)}
              FROM events.categories
              WHERE id = @CategoryId
              """;

        CategoryResponse? category = await dbConnection.QuerySingleOrDefaultAsync<CategoryResponse>(sql, request);

        if (category is null)
        {
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.CategoryId));
        }

        return category;
    }
}
