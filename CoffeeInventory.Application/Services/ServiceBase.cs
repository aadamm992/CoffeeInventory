using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public abstract class ServiceBase<TEntity>(IEntityRepositoryBase<TEntity> repository)
    where TEntity : class
{
    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<TEntity?> GetByNameAsync(string name)
    {
        return await repository.GetByNameAsync(name);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await repository.UpdateAsync(entity);
    }

    public async Task AddAsync(TEntity entity)
    {
        await repository.AddAsync(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await repository.DeleteAsync(entity);
    }
}
