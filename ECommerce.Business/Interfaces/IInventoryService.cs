using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Business.DTOs.Inventory;

namespace ECommerce.Business.Interfaces;

public interface IInventoryService
{
    Task<InventoryResponseDto?> GetByProductIdAsync(
        int productId);

    Task<InventoryResponseDto> CreateAsync(
        CreateInventoryDto dto);

    Task<bool> UpdateAsync(
        int productId,
        UpdateInventoryDto dto);
}