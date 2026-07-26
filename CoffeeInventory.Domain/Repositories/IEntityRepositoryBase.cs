namespace CoffeeInventory.Domain.Repositories;

public interface IEntityRepositoryBase<TEntity>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task UpdateAsync(TEntity entity);
    Task AddAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
