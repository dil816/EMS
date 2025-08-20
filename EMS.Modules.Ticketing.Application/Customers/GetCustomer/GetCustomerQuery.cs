using EMS.Common.Application.Messaging;

namespace EMS.Modules.Ticketing.Application.Customers.GetCustomer;
public sealed record GetCustomerQuery(Guid CustomerId) : IQuery<CustomerResponse>;
