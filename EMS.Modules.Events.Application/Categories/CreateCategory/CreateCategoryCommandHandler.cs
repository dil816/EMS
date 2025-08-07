using EMS.Modules.Events.Application.Abstractions.Data;
using EMS.Modules.Events.Application.Abstractions.Messaging;
using EMS.Modules.Events.Domain.Abstractions;
using EMS.Modules.Events.Domain.Categories;

namespace EMS.Modules.Events.Application.Categories.CreateCategory;
internal sealed class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.Create(request.Name);

        categoryRepository.Insert(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
