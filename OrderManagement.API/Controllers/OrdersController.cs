using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data;
namespace OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController
(IOrderService service, ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<List<OrderDto>> Get()
    {
        try
        {
            return await db.Orders
            .Include(x => x.Items)
            .Select(x => new OrderDto
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                OrderDate = x.OrderDate,
                TotalAmount = x.TotalAmount,

                Items = x.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            })
            .ToListAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var order = await db.Orders
            .Include(o => o.Items)
            .SingleOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return NotFound();

            var orderDto = new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,

                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Ok(orderDto);
        }
        catch (Exception)
        {

            throw;
        }
        
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto d)
    {
        try
        {
            var x = await service.CreateAsync(d);
            return CreatedAtAction(nameof(Get), new { id = x.Id }, x);
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
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id) =>
        await service.CancelAsync(id) ? NoContent() : NotFound();
}