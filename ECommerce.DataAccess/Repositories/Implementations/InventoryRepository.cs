using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.DataAccess.Context;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repositories.Implementations;

public class InventoryRepository : IInventoryRepository
{
    private readonly EcommerceDbContext _context;

    public InventoryRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<Inventory?> GetByProductIdAsync(int productId)
    {
        return await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
    }

    public async Task<Inventory> AddAsync(Inventory inventory)
    {
        _context.Inventories.Add(inventory);

        await _context.SaveChangesAsync();

        return inventory;
    }

    public async Task UpdateAsync(Inventory inventory)
    {
        _context.Inventories.Update(inventory);

        await _context.SaveChangesAsync();
    }
}