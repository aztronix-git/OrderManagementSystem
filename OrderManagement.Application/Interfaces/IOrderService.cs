using OrderManagement.Application.DTOs;
using OrderManagement.Core.Entities;
namespace OrderManagement.Application.Interfaces;

public interface IOrderService
{
    Task
<Order>
    CreateAsync(CreateOrderDto dto, CancellationToken ct = default); Task
    <bool>
        CancelAsync(int orderId, CancellationToken ct = default);
}