using ECommerce.Business.DTOs.Inventory;
using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(
        IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // GET: api/inventory/product/1
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetInventory(
        int productId)
    {
        var inventory =
            await _inventoryService
                .GetByProductIdAsync(productId);

        if (inventory == null)
        {
            return NotFound();
        }

        return Ok(inventory);
    }

    // POST: api/inventory
    [HttpPost]
    public async Task<IActionResult> CreateInventory(
        CreateInventoryDto dto)
    {
        var inventory =
            await _inventoryService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetInventory),
            new { productId = inventory.ProductId },
            inventory);
    }

    // PUT: api/inventory/product/1
    [HttpPut("product/{productId}")]
    public async Task<IActionResult> UpdateInventory(
        int productId,
        UpdateInventoryDto dto)
    {
        var updated =
            await _inventoryService.UpdateAsync(
                productId,
                dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}