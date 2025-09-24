using EMS.Common.Application.Authorization;
using EMS.Common.Application.Caching;
using EMS.Common.Domain;
using EMS.Modules.Users.IntegrationEvents;
using MassTransit;

namespace EMS.Modules.Ticketing.Infrastructure.Authorization;
internal sealed class PermissionService(
    IRequestClient<GetUserPermissionsRequest> requestClient,
    ICacheService cacheService) : IPermissionService
{
    private static readonly Error NotFound = Error.NotFound(nameof(PermissionService), "The user was not found");
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        // Cache service added for this operation faster 
        PermissionsResponse? permissionsResponse =
            await cacheService.GetAsync<PermissionsResponse>(CreateCacheKey(identityId));

        if (permissionsResponse is not null)
        {
            return permissionsResponse;
        }

        // MassTransit Added for get user Permission form user Module due to
        // microservice extraction
        var request = new GetUserPermissionsRequest(identityId);

        Response<PermissionsResponse, Error> response =
            await requestClient.GetResponse<PermissionsResponse, Error>(request);

        if (response.Is(out Response<Error> errorResponse))
        {
            return Result.Failure<PermissionsResponse>(errorResponse.Message);
        }

        if (response.Is(out Response<PermissionsResponse> permissionResponse))
        {
            // set to permission value to CacheService storage
            await cacheService.SetAsync(
                CreateCacheKey(identityId),
                permissionResponse.Message,
                CacheExpiration);

            return permissionResponse.Message;
        }

        return Result.Failure<PermissionsResponse>(NotFound);
    }

    private static string CreateCacheKey(string identityId) => $"user-permissions:{identityId}";
}
