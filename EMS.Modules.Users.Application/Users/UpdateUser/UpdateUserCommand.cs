using EMS.Common.Application.Messaging;

namespace EMS.Modules.Users.Application.Users.UpdateUser;
public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand;

