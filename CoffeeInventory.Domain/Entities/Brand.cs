namespace CoffeeInventory.Domain.Entities;

public class Brand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<Coffee> Coffees { get; set; } = new HashSet<Coffee>();
}
