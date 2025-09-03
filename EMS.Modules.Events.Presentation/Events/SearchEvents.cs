﻿using EMS.Common.Domain;
using EMS.Common.Presentation.ApiResults;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Events.Application.Events.SearchEvents;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EMS.Modules.Events.Presentation.Events;
internal sealed class SearchEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/search",
            async (
            ISender sender,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int page = 0,
            int pageSize = 15) =>
            {
                Result<SearchEventsResponse> result = await sender.Send(
                    new SearchEventsQuery(categoryId, startDate, endDate, page, pageSize));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
        .RequireAuthorization(Permissions.SearchEvents)
        .WithTags(Tags.Events);
    }
}

