using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Core.Entities;

namespace OrderManagement.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
        public class DashboardController(
        IRepository<Order> orderRepo,
        IRepository<Product> productRepo,
        IRepository<Customer> customerRepo) : ControllerBase
        {
            [HttpGet("summary")]
            public async Task<IActionResult> GetSummary()
            {
                try
                {
                    var totalOrders = await orderRepo.CountAsync();
                    var totalProducts = await productRepo.CountAsync();
                    var totalCustomers = await customerRepo.CountAsync();

                    return Ok(new
                    {
                        TotalOrders = totalOrders,
                        TotalProducts = totalProducts,
                        TotalCustomers = totalCustomers
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500,
                        "An error occurred while retrieving dashboard summary.");
                }
            }
        }


