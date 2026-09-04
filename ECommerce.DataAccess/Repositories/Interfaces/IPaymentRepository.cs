using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Domain.Entities;

namespace ECommerce.DataAccess.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByOrderIdAsync(int orderId);

    Task<Payment> AddAsync(Payment payment);

    Task UpdateAsync(Payment payment);
}