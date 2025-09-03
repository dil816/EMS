using EMS.Common.Application.Exceptions;
using EMS.Common.Infrastructure.Authentication;
using EMS.Modules.Ticketing.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace EMS.Modules.Ticketing.Infrastructure.Authentication;

internal sealed class CustomerContext(IHttpContextAccessor httpContextAccessor) : ICustomerContext
{
    public Guid CustomerId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new EmsException("User identifier is unavailable");
}
