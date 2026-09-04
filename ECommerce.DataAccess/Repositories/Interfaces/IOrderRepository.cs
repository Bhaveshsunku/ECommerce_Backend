using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities;

namespace ECommerce.DataAccess.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);

    Task<Order?> GetByIdAsync(int id);

    Task<List<Order>> GetByUserIdAsync(int userId);

    Task UpdateAsync(Order order);
    Task<List<Order>> GetPendingOrdersOlderThanAsync(DateTime cutoff);

}