using System.Reflection;
using EMS.Modules.Attendance.Domain.Attendees;
using EMS.Modules.Attendance.Infrastructure;

namespace EMS.Modules.Attendance.ArchitectureTests.Abstractions;

internal static class BaseTest
{
    public static readonly Assembly ApplicationAssembly = typeof(Attendance.Application.AssemblyReference).Assembly;

    public static readonly Assembly DomainAssembly = typeof(Attendee).Assembly;

    public static readonly Assembly InfrastructureAssembly = typeof(AttendanceModule).Assembly;

    public static readonly Assembly PresentationAssembly = typeof(Attendance.Presentation.AssemblyReference).Assembly;
}
