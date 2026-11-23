using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Core.Entities;
namespace OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController
(IRepository<Customer> repo) : ControllerBase
{
    [HttpGet]
    public Task<List<Customer>> Get() => repo.GetAllAsync();
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) => await repo.GetByIdAsync(id) is { }
    x ? Ok(x) : NotFound();
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto d)
    {
        var x = new Customer { Name = d.Name, Email = d.Email, Phone = d.Phone };
        await repo.AddAsync(x);
        await repo.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = x.Id }, x);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateCustomerDto d)
    {
        var x = await repo.GetByIdAsync(id);
        if (x is null)
            return NotFound();
        x.Name = d.Name;
        x.Email = d.Email;
        x.Phone = d.Phone;
        repo.Update(x);
        await repo.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var x = await repo.GetByIdAsync(id);
        if (x is null)
            return NotFound();
        repo.Delete(x);
        await repo.SaveChangesAsync();
        return NoContent();
    }
}