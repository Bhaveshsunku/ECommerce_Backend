using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Business.DTOs.Payments;

public class CheckoutDto
{
    [Required]
    public string PaymentMethod { get; set; } = string.Empty;
}