using EMS.Common.Application.Exceptions;
using EMS.Common.Infrastructure.Authentication;
using EMS.Modules.Attendance.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace EMS.Modules.Attendance.Infrastructure.Authentication;
internal sealed class AttendanceContext(IHttpContextAccessor httpContextAccessor) : IAttendanceContext
{
    public Guid AttendeeId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new EmsException("User identifier is unavailable");
}

