using EMS.Common.Application.Authorization;
using EMS.Common.Domain;
using EMS.Modules.Users.IntegrationEvents;
using MassTransit;

namespace EMS.Modules.Users.Presentation.Users;
public sealed class GetUserPermissionsRequestConsumer(IPermissionService permissionService) : IConsumer<GetUserPermissionsRequest>
{
    public async Task Consume(ConsumeContext<GetUserPermissionsRequest> context)
    {
        Result<PermissionsResponse> result =
            await permissionService.GetUserPermissionsAsync(context.Message.IdentityId);

        if (result.IsSuccess)
        {
            await context.RespondAsync(result.Value);
        }
        else
        {
            await context.RespondAsync(result.Error);
        }
    }
}
