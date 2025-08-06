namespace EMS.Modules.Events.Domain.Categories;
public interface ICategoryRepository
{
    Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken);

    void Insert(Category category);
}
