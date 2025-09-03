using EMS.Common.Domain;
using EMS.Common.Presentation.ApiResults;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Ticketing.Application.Abstractions.Authentication;
using EMS.Modules.Ticketing.Application.Carts.ClearCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EMS.Modules.Ticketing.Presentation.Carts;
internal sealed class ClearCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("carts", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result result = await sender.Send(new ClearCartCommand(customerContext.CustomerId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.RemoveFromCart)
        .WithTags(Tags.Carts);
    }
}
