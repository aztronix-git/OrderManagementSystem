using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
namespace OrderManagement.Infrastructure.Data;

public class ApplicationDbContext
(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<Inventory>().HasIndex(x => x.ProductId).IsUnique();
        b.Entity<Inventory>()
            .HasOne(x => x.Product)
            .WithOne(x => x.Inventory)
            .HasForeignKey<Inventory>(x => x.ProductId);
        b.Entity<Payment>().HasIndex(x => x.OrderId).IsUnique();
        b.Entity<Order>().Property(x => x.TotalAmount).HasPrecision(18, 2);
        b.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);
        b.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        b.Entity<Payment>().Property(x => x.Amount).HasPrecision(18, 2);
    }
}