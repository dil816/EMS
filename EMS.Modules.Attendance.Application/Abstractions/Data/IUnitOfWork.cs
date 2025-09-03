﻿namespace EMS.Modules.Attendance.Application.Abstractions.Data;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
