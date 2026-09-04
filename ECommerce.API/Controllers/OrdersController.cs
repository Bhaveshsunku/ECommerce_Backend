using System.Security.Claims;
using ECommerce.Business.DTOs.Orders;
using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        CreateOrderDto dto)
    {
        var userId = GetUserId();

        var order =
            await _orderService.CreateAsync(
                userId,
                dto);

        return CreatedAtAction(
            nameof(GetOrder),
            new { id = order.Id },
            order);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();

        var orders =
            await _orderService.GetMyOrdersAsync(
                userId);

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var userId = GetUserId();

        var order =
            await _orderService.GetByIdAsync(
                id,
                userId);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var userId = GetUserId();

        var cancelled =
            await _orderService.CancelAsync(
                id,
                userId);

        if (!cancelled)
        {
            return NotFound();
        }

        return NoContent();
    }

    private int GetUserId()
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!int.TryParse(
                userIdClaim,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "Invalid user identity.");
        }

        return userId;
    }
}