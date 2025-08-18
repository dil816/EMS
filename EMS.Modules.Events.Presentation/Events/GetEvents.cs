using EMS.Common.Domain;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Events.Application.Events.GetEvents;
using EMS.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EMS.Modules.Events.Presentation.Events;
internal class GetEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async (ISender sender) =>
        {
            Result<IReadOnlyCollection<EventResponse>> result = await sender.Send(new GetEventsQuery());

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Events);
    }
}

