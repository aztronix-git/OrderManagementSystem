using OrderManagement.Core.Entities;
namespace OrderManagement.Application.Interfaces;

public interface IInventoryService
{
    Task
<Inventory>
    ReceiveStockAsync(int productId, int quantity, string reference, CancellationToken ct = default); Task
    <Inventory>
        IssueStockAsync(int productId, int quantity, string reference, CancellationToken ct = default); Task
        <List<Inventory>
            > GetLowStockAsync(CancellationToken ct = default);
}