﻿using EMS.Modules.Events.Application.Abstractions.Data;
using EMS.Modules.Events.Application.Abstractions.Messaging;
using EMS.Modules.Events.Domain.Abstractions;
using EMS.Modules.Events.Domain.Categories;

namespace EMS.Modules.Events.Application.Categories.UpdateCategory;
internal sealed class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(request.CategoryId));
        }
        category.ChangeName(request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
