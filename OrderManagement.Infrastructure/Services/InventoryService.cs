using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data;
namespace OrderManagement.Infrastructure.Services;

public class InventoryService
(ApplicationDbContext db) : IInventoryService
{
    public async Task<Inventory> ReceiveStockAsync(int productId, int quantity, string reference,
                                                   CancellationToken ct = default)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        _ = await db.Products.FindAsync([productId], ct) ??
            throw new KeyNotFoundException("Product not found.");
        var inv = await db.Inventories.SingleOrDefaultAsync(x => x.ProductId == productId, ct);
        if (inv is null)
        {
            inv = new Inventory { ProductId = productId, QuantityAvailable = 0 };
            db.Inventories.Add(inv);
        }
        inv.QuantityAvailable += quantity;
        inv.UpdatedAt = DateTime.UtcNow;
        db.StockMovements.Add(new StockMovement
        {
            ProductId = productId,
            Quantity = quantity,
            MovementType = "IN",
            Reference = reference
        });
        await db.SaveChangesAsync(ct);
        return inv;
    }
    public async Task<Inventory> IssueStockAsync(int productId, int quantity, string reference,
                                                 CancellationToken ct = default)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        var inv = await db.Inventories.SingleOrDefaultAsync(x => x.ProductId == productId, ct) ??
                  throw new KeyNotFoundException("Inventory not found.");
        if (inv.QuantityAvailable < quantity)
            throw new InvalidOperationException("Insufficient stock.");
        inv.QuantityAvailable -= quantity;
        inv.UpdatedAt = DateTime.UtcNow;
        db.StockMovements.Add(new StockMovement
        {
            ProductId = productId,
            Quantity = quantity,
            MovementType = "OUT",
            Reference = reference
        });
        await db.SaveChangesAsync(ct);
        return inv;
    }
    public Task<List<Inventory>> GetLowStockAsync(CancellationToken ct = default) =>
        db.Inventories.Include(x => x.Product)
            .Where(x => x.Product != null && x.QuantityAvailable <= x.Product.ReorderLevel)
            .ToListAsync(ct);
}