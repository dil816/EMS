using Microsoft.AspNetCore.Routing;

namespace EMS.Common.Presentation.EndPoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
