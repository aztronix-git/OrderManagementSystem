namespace OrderManagement.Core.Entities;

public class Inventory
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantityReserved { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}