using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Domain.Entities;

namespace ECommerce.DataAccess.Repositories.Interfaces;

public interface IInventoryRepository
{
    Task<Inventory?> GetByProductIdAsync(int productId);

    Task<Inventory> AddAsync(Inventory inventory);

    Task UpdateAsync(Inventory inventory);
}