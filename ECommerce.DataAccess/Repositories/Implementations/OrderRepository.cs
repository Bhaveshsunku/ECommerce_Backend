using ECommerce.DataAccess.Context;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repositories.Implementations;

public class OrderRepository : IOrderRepository
{
    private readonly EcommerceDbContext _context;

    public OrderRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // Create Order
    // ==========================================

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return order;
    }

    // ==========================================
    // Get Order By ID
    // ==========================================

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    // ==========================================
    // Get Orders By User
    // ==========================================

    public async Task<List<Order>> GetByUserIdAsync(
        int userId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    // ==========================================
    // Update Order
    // ==========================================

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);

        await _context.SaveChangesAsync();
    }

    // ==========================================
    // Get Old Pending Orders
    // ==========================================

    public async Task<List<Order>> GetPendingOrdersOlderThanAsync(
        DateTime cutoff)
    {
        return await _context.Orders
            .Where(o =>
                o.Status == OrderStatus.Pending &&
                o.OrderDate < cutoff)
            .ToListAsync();
    }
}