namespace OrderManagement.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public decimal Price { get; set; }
    public int ReorderLevel { get; set; } = 5;
    public Inventory? Inventory { get; set; }
}