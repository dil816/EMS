using EMS.Common.Domain;
using EMS.Common.Presentation.ApiResults;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Ticketing.Application.Abstractions.Authentication;
using EMS.Modules.Ticketing.Application.Carts;
using EMS.Modules.Ticketing.Application.Carts.GetCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EMS.Modules.Ticketing.Presentation.Carts;
internal sealed class GetCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("carts", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result<Cart> result = await sender.Send(new GetCartQuery(customerContext.CustomerId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetCart)
        .WithTags(Tags.Carts);
    }
}
