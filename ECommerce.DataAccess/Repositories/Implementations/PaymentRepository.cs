using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.DataAccess.Context;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repositories.Implementations;

public class PaymentRepository : IPaymentRepository
{
    private readonly EcommerceDbContext _context;

    public PaymentRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);

        await _context.SaveChangesAsync();
    }
}