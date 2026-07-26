namespace CoffeeInventory.Domain.Entities;

public class CupSize
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int VolumeMl { get; set; }
    
    public ICollection<Coffee> Coffees { get; set; } = new HashSet<Coffee>();
}
