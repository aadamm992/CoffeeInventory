namespace CoffeeInventory.Domain.Entities;

public class CapsuleType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<Coffee> Coffees = new HashSet<Coffee>();

    public override string ToString()
    {
        return Name;
    }
}
