namespace OrderManagement.Core.Entities;

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = "IN";
    public string Reference { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}