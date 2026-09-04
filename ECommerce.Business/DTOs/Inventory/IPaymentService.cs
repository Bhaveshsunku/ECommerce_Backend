using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Payments;

namespace ECommerce.Business.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponseDto> CheckoutAsync(
        int orderId,
        int userId,
        CheckoutDto dto);
}