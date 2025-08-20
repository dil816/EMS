using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Customers.CreateCustomer;
public sealed record CreateCustomerCommand(Guid CustomerId, string Email, string FirstName, string LastName)
    : ICommand;
