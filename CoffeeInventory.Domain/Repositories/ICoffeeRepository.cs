using CoffeeInventory.Domain.Entities;

namespace CoffeeInventory.Domain.Repositories;

public interface ICoffeeRepository : IEntityRepositoryBase<Coffee>
{
    Task<Coffee?> GetByIdAsync(Guid id);
    Task<Coffee?> GetByNameAsync(string name);
    Task<IReadOnlyList<Coffee>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<IReadOnlyList<Coffee>> GetByNamesAsync(IEnumerable<string> names);
    Task AddAsync(Guid brandId, Coffee coffee, Guid? capsuleTypeId = null, IEnumerable<Guid>? cupSizeIds = null);
}
