using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data;
namespace OrderManagement.Infrastructure.Services;

public class OrderService
(ApplicationDbContext db) : IOrderService
{
    public async Task<Order> CreateAsync(CreateOrderDto dto, CancellationToken ct = default)
    {
        if (dto.Items.Count == 0)
            throw new ArgumentException("At least one order item is required.");
        if (!await db.Customers.AnyAsync(x => x.Id == dto.CustomerId, ct))
            throw new KeyNotFoundException("Customer not found.");
        await using var tx = await db.Database.BeginTransactionAsync(ct);
        var order = new Order { CustomerId = dto.CustomerId };
        foreach (var line in dto.Items)
        {
            if (line.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
            var product = await db.Products.SingleOrDefaultAsync(x => x.Id == line.ProductId, ct) ??
                          throw new KeyNotFoundException($"Product {line.ProductId} not found.");
            var inv =
                await db.Inventories.SingleOrDefaultAsync(x => x.ProductId == line.ProductId, ct) ??
                throw new InvalidOperationException($"No inventory for {product.Name}.");
            if (inv.QuantityAvailable < line.Quantity)
                throw new InvalidOperationException($"Insufficient stock for {product.Name}.");
            inv.QuantityAvailable -= line.Quantity;
            inv.UpdatedAt = DateTime.UtcNow;
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = line.Quantity,
                UnitPrice = product.Price
            });
            order.TotalAmount += product.Price * line.Quantity;
            db.StockMovements.Add(
                new StockMovement
                {
                    ProductId = product.Id,
                    Quantity = line.Quantity,
                    MovementType = "OUT",
                    Reference = "Order pending"
                });
        }
        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);
        foreach (var movement in db.ChangeTracker.Entries<StockMovement>().Where(
                     e => e.State == EntityState.Added))
            if (movement.Entity.Reference == "Order pending")
                movement.Entity.Reference = $"Order #{order.Id}";
        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return order;
    }
    public async Task<bool> CancelAsync(int orderId, CancellationToken ct = default)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);
        var order =
            await db.Orders.Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == orderId, ct);
        if (order is null)
            return false;
        if (order.Status == "Cancelled")
            return true;
        foreach (var item in order.Items)
        {
            var inv = await db.Inventories.SingleAsync(x => x.ProductId == item.ProductId, ct);
            inv.QuantityAvailable += item.Quantity;
            inv.UpdatedAt = DateTime.UtcNow;
            db.StockMovements.Add(
                new StockMovement
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    MovementType = "IN",
                    Reference = $"Cancellation of order #{order.Id}"
                });
        }
        order.Status = "Cancelled";
        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return true;
    }
}