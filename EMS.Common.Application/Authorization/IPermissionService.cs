using EMS.Common.Domain;

namespace EMS.Common.Application.Authorization;
public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}

