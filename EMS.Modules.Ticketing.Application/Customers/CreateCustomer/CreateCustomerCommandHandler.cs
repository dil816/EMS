using EMS.Common.Application.Messaging;
using EMS.Common.Domain;
using EMS.Modules.Ticketing.Application.Abstractions;
using EMS.Modules.Ticketing.Domain.Customers;

namespace EMS.Modules.Ticketing.Application.Customers.CreateCustomer;

internal sealed class CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCustomerCommand>
{
    public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(request.CustomerId, request.Email, request.FirstName, request.LastName);

        customerRepository.Insert(customer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
