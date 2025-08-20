using EMS.Common.Domain;
using EMS.Modules.Users.Application.Users.GetUser;
using EMS.Modules.Users.PublicApi;
using MediatR;
using UserResponse = EMS.Modules.Users.PublicApi.UserResponse;

namespace EMS.Modules.Users.Infrastructure.PublicApi;
internal sealed class UsersApi(ISender sender) : IUsersApi
{
    public async Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        Result<Application.Users.GetUser.UserResponse> result =
            await sender.Send(new GetUserQuery(userId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        return new UserResponse(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName);
    }
}
