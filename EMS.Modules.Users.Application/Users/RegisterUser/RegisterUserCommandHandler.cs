using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.PublicApi;
using EMS.Modules.Users.Application.Abstractions.Data;
using EMS.Modules.Users.Domain.Users;

namespace EMS.Modules.Users.Application.Users.RegisterUser;
internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ITicketingApi ticketingApi)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Email, request.FirstName, request.LastName);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await ticketingApi.CreateCustomerAsync(user.Id, user.Email, user.FirstName, user.LastName, cancellationToken);

        return user.Id;
    }
}
