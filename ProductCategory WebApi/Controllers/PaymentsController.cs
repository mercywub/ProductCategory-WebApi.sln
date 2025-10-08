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
        private readonly PaymentService _service;
        private readonly IPaymentService _paymentService;

        public PaymentsController(PaymentService service, IPaymentService paymentService)
        {
            _service = service;
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto dto)
        {
            try
            {
                var payment = await _paymentService.ProcessPaymentAsync(dto);
                return Ok(new
                {
                    Message = "Payment processed successfully",
                    Payment = payment
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pay")]
        public async Task<IActionResult> PayOrder([FromBody] PaymentRequestDto dto)
        {
            var result = await _service.ProcessPaymentAsync(dto.OrderId, dto.Amount, dto.PaymentMethod);
            if (!result) return BadRequest("Payment failed or order not found.");

            return Ok("Payment successful. Email notification sent.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentCreateDto dto)
        {
            var payment = await _service.CreatePaymentAsync(dto);
            return Ok(payment);
        }

        [HttpGet("by-order/{orderId:guid}")]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var payments = await _service.GetPaymentsByOrderAsync(orderId);
            return Ok(payments);
        }
    }
}
