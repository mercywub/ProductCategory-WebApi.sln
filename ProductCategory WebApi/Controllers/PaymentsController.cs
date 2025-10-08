using Microsoft.AspNetCore.Mvc;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Application.Services;
using ProductCategory_WebApi.Domain.Interfaces;

namespace ProductCategory_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("stripe")]
        public async Task<IActionResult> CreateStripePayment([FromBody] StripePaymentRequestDto request)
        {
            var payment = await _paymentService.CreateStripePaymentAsync(request);
            return Ok(payment); // Return PaymentIntent info for frontend to confirm payment
        }
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            var payments = await _paymentService.GetPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(Guid id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(PaymentCreateDto dto)
        {
            var payment = await _paymentService.CreatePaymentAsync(dto);
            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid id, UpdatePaymentDto dto)
        {
            await _paymentService.UpdatePaymentStatusAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return NoContent();
        }
    }
}
