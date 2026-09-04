using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Inventory;
using ECommerce.Business.Interfaces;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Business.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IProductRepository _productRepository;

    public InventoryService(
        IInventoryRepository inventoryRepository,
        IProductRepository productRepository)
    {
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
    }

    // Get inventory by product ID
    public async Task<InventoryResponseDto?> GetByProductIdAsync(
        int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentException(
                "Product ID must be greater than zero.");
        }

        var inventory =
            await _inventoryRepository.GetByProductIdAsync(productId);

        if (inventory == null)
        {
            return null;
        }

        return MapToResponseDto(inventory);
    }

    // Create inventory for a product
    public async Task<InventoryResponseDto> CreateAsync(
        CreateInventoryDto dto)
    {
        if (dto.ProductId <= 0)
        {
            throw new ArgumentException(
                "Product ID must be greater than zero.");
        }

        if (dto.Quantity < 0)
        {
            throw new ArgumentException(
                "Quantity cannot be negative.");
        }

        // Check whether product exists
        var product =
            await _productRepository.GetByIdAsync(dto.ProductId);

        if (product == null)
        {
            throw new ArgumentException(
                "Product does not exist.");
        }

        // Check whether inventory already exists
        var existingInventory =
            await _inventoryRepository
                .GetByProductIdAsync(dto.ProductId);

        if (existingInventory != null)
        {
            throw new InvalidOperationException(
                "Inventory already exists for this product.");
        }

        var inventory = new Inventory
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UpdatedAt = DateTime.UtcNow
        };

        var createdInventory =
            await _inventoryRepository.AddAsync(inventory);

        return MapToResponseDto(createdInventory);
    }

    // Update inventory quantity
    public async Task<bool> UpdateAsync(
        int productId,
        UpdateInventoryDto dto)
    {
        if (productId <= 0)
        {
            throw new ArgumentException(
                "Product ID must be greater than zero.");
        }

        if (dto.Quantity < 0)
        {
            throw new ArgumentException(
                "Quantity cannot be negative.");
        }

        var inventory =
            await _inventoryRepository
                .GetByProductIdAsync(productId);

        if (inventory == null)
        {
            return false;
        }

        inventory.Quantity = dto.Quantity;
        inventory.UpdatedAt = DateTime.UtcNow;

        await _inventoryRepository.UpdateAsync(inventory);

        return true;
    }

    // Convert entity to response DTO
    private static InventoryResponseDto MapToResponseDto(
        Inventory inventory)
    {
        return new InventoryResponseDto
        {
            Id = inventory.Id,
            ProductId = inventory.ProductId,
            Quantity = inventory.Quantity,
            UpdatedAt = inventory.UpdatedAt
        };
    }
}