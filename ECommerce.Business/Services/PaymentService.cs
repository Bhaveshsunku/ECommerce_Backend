using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Business.DTOs.Payments;
using ECommerce.Business.Interfaces;
using ECommerce.DataAccess.Repositories.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Business.Services;

public class PaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentResponseDto> CheckoutAsync(
        int orderId,
        int userId,
        CheckoutDto dto)
    {
        var order =
            await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new ArgumentException(
                "Order not found.");
        }

        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "You cannot checkout this order.");
        }

        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending orders can be checked out.");
        }

        var existingPayment =
            await _paymentRepository.GetByOrderIdAsync(orderId);

        if (existingPayment != null)
        {
            throw new InvalidOperationException(
                "Payment already exists for this order.");
        }

        if (string.IsNullOrWhiteSpace(dto.PaymentMethod))
        {
            throw new ArgumentException(
                "Payment method is required.");
        }

        // Mock payment processing
        var paymentSuccessful = true;

        var payment = new Payment
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            PaymentStatus = paymentSuccessful
                ? PaymentStatus.Successful
                : PaymentStatus.Failed,
            TransactionId = paymentSuccessful
                ? $"MOCK-{Guid.NewGuid():N}"
                : null,
            PaymentDate = paymentSuccessful
                ? DateTime.UtcNow
                : null
        };

        var createdPayment =
            await _paymentRepository.AddAsync(payment);

        if (paymentSuccessful)
        {
            order.Status = OrderStatus.Confirmed;

            await _orderRepository.UpdateAsync(order);
        }

        return new PaymentResponseDto
        {
            PaymentId = createdPayment.Id,
            OrderId = createdPayment.OrderId,
            Amount = createdPayment.Amount,
            PaymentStatus = createdPayment.PaymentStatus,
            TransactionId = createdPayment.TransactionId,
            PaymentDate = createdPayment.PaymentDate
        };
    }
}