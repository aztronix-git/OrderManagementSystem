using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Infrastructure.Data;
namespace OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController
(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public Task<List<Supplier>> Get() => db.Suppliers.ToListAsync();
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) => await db.Suppliers.FindAsync(id) is { }
    x ? Ok(x) : NotFound();
    [HttpPost]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        supplier.Id = 0;
        db.Suppliers.Add(supplier);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = supplier.Id }, supplier);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Supplier input)
    {
        var x = await db.Suppliers.FindAsync(id);
        if (x is null)
            return NotFound();
        x.Name = input.Name;
        x.Email = input.Email;
        x.Phone = input.Phone;
        await db.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var x = await db.Suppliers.FindAsync(id);
        if (x is null)
            return NotFound();
        db.Suppliers.Remove(x);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
