using OrderManagement.Core.Entities;

namespace OrderManagement.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(CancellationToken ct = default);   
    Task<List<Product>> GetAllAsync(int pageNumber, int pageSize);
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync(CancellationToken ct = default);

    Task<int> CountAsync();


}