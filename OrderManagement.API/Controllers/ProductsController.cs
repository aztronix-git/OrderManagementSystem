using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Core.Entities;

namespace OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController
(IRepository<Product> repo) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get(int pageNumber = 1,int pageSize = 5)
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters.");

            pageSize = Math.Min(pageSize, 100);

            var products = await repo.GetAllAsync(pageNumber, pageSize);

            return Ok(products);
        }
        catch (Exception ex)
        {
            //logger.LogError(ex, "Error retrieving products");

            return StatusCode(500, "An error occurred while retrieving products.");
        }
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) => await repo.GetByIdAsync(id) is { }
    x ? Ok(x) : NotFound();
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto d)
    {
        if (d.Price < 0 || d.ReorderLevel < 0)
            return BadRequest("Price and reorder level cannot be negative.");
        var x = new Product
        {
            Name = d.Name,
            Price = d.Price,
            ReorderLevel = d.ReorderLevel
        };
        await repo.AddAsync(x);
        await repo.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = x.Id }, x);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateProductDto d)
    {
        var x = await repo.GetByIdAsync(id);
        if (x is null)
            return NotFound();
        x.Name = d.Name;
        x.Price = d.Price;
        x.ReorderLevel = d.ReorderLevel;
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