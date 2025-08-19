using EMS.Common.Application.Messaging;

namespace EMS.Modules.Users.Application.Users.GetUser;
public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
