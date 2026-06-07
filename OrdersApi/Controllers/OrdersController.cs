// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.DTO;
using OrdersApi.DTOs;

namespace OrdersApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/orders/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _db.Orders
            .Include(o => o.Client)
            .Include(o => o.Status)
            .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            return NotFound($"Zamówienie o ID {id} nie istnieje.");

        var dto = new OrderResponseDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            FulfilledAt = order.FulfilledAt,
            Status = order.Status.Name,
            Client = new ClientDto
            {
                FirstName = order.Client.FirstName,
                LastName = order.Client.LastName
            },
            Products = order.ProductOrders.Select(po => new ProductDto
            {
                Name = po.Product.Name,
                Price = po.Product.Price,
                Amount = po.Amount
            }).ToList()
        };

        return Ok(dto);
    }
    // PUT /api/orders/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDto dto)
    {
        var order = await _db.Orders
            .Include(o => o.ProductOrders)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            return NotFound($"Zamówienie o ID {id} nie istnieje.");

        if (order.FulfilledAt is not null)
            return BadRequest("Zamówienie zostało już zrealizowane i nie można go zmienić.");

        var status = await _db.Statuses
            .FirstOrDefaultAsync(s => s.Name == dto.StatusName);

        if (status is null)
            return NotFound($"Status '{dto.StatusName}' nie istnieje.");

        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.ProductOrders.RemoveRange(order.ProductOrders);

            order.StatusId = status.Id;
            order.FulfilledAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Błąd wewnętrzny: {ex.Message}");
        }

        return NoContent(); // 204
    }
}