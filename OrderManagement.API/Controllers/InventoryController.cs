using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Infrastructure.Data;
namespace OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController
(IInventoryService service, ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public Task<List<OrderManagement.Core.Entities.Inventory>> Get() =>
        db.Inventories.Include(x => x.Product).ToListAsync();
    [HttpGet("low-stock")]
    public Task<List<OrderManagement.Core.Entities.Inventory>> LowStock() =>
        service.GetLowStockAsync();
    [HttpPost("receive")]
    public async Task<IActionResult> Receive(StockRequestDto d)
    {
        try
        {
            return Ok(await service.ReceiveStockAsync(d.ProductId, d.Quantity, d.Reference));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    [HttpPost("issue")]
    public async Task<IActionResult> Issue(StockRequestDto d)
    {
        try
        {
            return Ok(await service.IssueStockAsync(d.ProductId, d.Quantity, d.Reference));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidOperationException e)
        {
            return Conflict(new { message = e.Message });
        }
    }
    [HttpGet("movements")]
    public Task<List<OrderManagement.Core.Entities.StockMovement>> Movements() =>
        db.StockMovements.Include(x => x.Product).OrderByDescending(x => x.CreatedAt).ToListAsync();
}