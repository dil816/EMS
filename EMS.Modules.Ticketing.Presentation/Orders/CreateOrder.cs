using EMS.Common.Domain;
using EMS.Common.Presentation.ApiResults;
using EMS.Common.Presentation.EndPoints;
using EMS.Modules.Ticketing.Application.Abstractions.Authentication;
using EMS.Modules.Ticketing.Application.Orders.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EMS.Modules.Ticketing.Presentation.Orders;
internal sealed class CreateOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result result = await sender.Send(new CreateOrderCommand(customerContext.CustomerId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
         .RequireAuthorization(Permissions.CreateOrder)
         .WithTags(Tags.Orders);
    }
}
