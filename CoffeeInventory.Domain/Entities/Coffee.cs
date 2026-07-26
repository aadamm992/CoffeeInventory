namespace CoffeeInventory.Domain.Entities;

public class Coffee
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public int Consumed { get; set; }
    public bool IsDecaffeinated { get; set; }
    
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    
    public Guid? CapsuleTypeId { get; set; }
    public CapsuleType? CapsuleType { get; set; }
    
    public ICollection<CupSize> CupSizes { get; set; } = new HashSet<CupSize>();
}
