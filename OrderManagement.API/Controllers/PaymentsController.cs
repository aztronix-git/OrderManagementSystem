using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data;
namespace OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController
(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public Task<List<Payment>> Get() => db.Payments.ToListAsync();
    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentDto d)
    {
        var order = await db.Orders.FindAsync(d.OrderId);
        if (order is null)
            return NotFound("Order not found.");
        if (d.Amount <= 0 || d.Amount != order.TotalAmount)
            return BadRequest("Payment amount must equal the order total and be positive.");
        if (await db.Payments.AnyAsync(x => x.OrderId == d.OrderId))
            return Conflict("Payment already exists for this order.");
        var p = new Payment
        {
            OrderId = d.OrderId,
            Amount = d.Amount,
            Method = d.Method,
            Status = "Paid",
            PaidAt = DateTime.UtcNow
        };
        db.Payments.Add(p);
        await db.SaveChangesAsync();
        return Ok(p);
    }
}