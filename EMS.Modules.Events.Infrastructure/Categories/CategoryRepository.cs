﻿using EMS.Modules.Events.Domain.Categories;
using EMS.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EMS.Modules.Events.Infrastructure.Categories;
internal sealed class CategoryRepository(EventsDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
        context.Categories.Add(category);
    }
}
