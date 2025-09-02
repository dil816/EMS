using EMS.Common.Application.Authorization;
using EMS.Common.Application.Messaging;

namespace EMS.Modules.Users.Application.Users.GetUserPermissions;
public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;

