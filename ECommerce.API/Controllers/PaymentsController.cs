using System.Security.Claims;
using ECommerce.Business.DTOs.Payments;
using ECommerce.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("checkout/{orderId}")]
    public async Task<IActionResult> Checkout(
        int orderId,
        CheckoutDto dto)
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var payment =
            await _paymentService.CheckoutAsync(
                orderId,
                userId,
                dto);

        return Ok(payment);
    }
}