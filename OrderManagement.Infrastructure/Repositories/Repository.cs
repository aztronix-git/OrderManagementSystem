using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data;
namespace OrderManagement.Infrastructure.Repositories;

public class Repository<T>(ApplicationDbContext db) : IRepository<T>
    where T : class
{
    public Task<List<T>> GetAllAsync(CancellationToken ct = default) => db.Set<T>().ToListAsync(ct);
    public Task<T?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Set<T>().FindAsync([id], ct).AsTask();
    public Task AddAsync(T entity, CancellationToken ct = default) =>
        db.Set<T>().AddAsync(entity, ct).AsTask();
    public void Update(T entity) => db.Set<T>().Update(entity);
    public void Delete(T entity) => db.Set<T>().Remove(entity);
    public Task SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);

    public async Task<List<Product>> GetAllAsync(
     int pageNumber, int pageSize)
    {
        return await db.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await db.Set<T>().CountAsync();
    }
}