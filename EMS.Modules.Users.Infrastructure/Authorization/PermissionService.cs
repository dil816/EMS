using EMS.Common.Application.Authorization;
using EMS.Common.Domain;
using EMS.Modules.Users.Application.Users.GetUserPermissions;
using MediatR;

namespace EMS.Modules.Users.Infrastructure.Authorization;
internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
