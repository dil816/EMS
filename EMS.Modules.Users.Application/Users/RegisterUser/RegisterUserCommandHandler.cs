using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Users.Application.Abstractions.Data;
using EMS.Modules.Users.Domain.Users;

namespace EMS.Modules.Users.Application.Users.RegisterUser;
internal sealed class RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Email, request.FirstName, request.LastName);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
