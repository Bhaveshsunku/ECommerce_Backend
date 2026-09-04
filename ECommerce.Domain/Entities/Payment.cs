using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public Order Order { get; set; } = null!;
}