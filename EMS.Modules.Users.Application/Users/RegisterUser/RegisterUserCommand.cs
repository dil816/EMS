using EMS.Common.Application.Messaging;

namespace EMS.Modules.Users.Application.Users.RegisterUser;
public sealed record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : ICommand<Guid>;
