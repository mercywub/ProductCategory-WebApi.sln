using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using Stripe;

namespace ProductCategory_WebApi.Application.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _configuration = configuration;

            var secretKey = _configuration["Stripe:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("Stripe Secret Key is missing. Check your appsettings.json.");

            StripeConfiguration.ApiKey = secretKey;
        }

        /// <summary>
        /// Creates a Stripe PaymentIntent and saves it to the database
        /// </summary>
        public async Task<PaymentDto> CreateStripePaymentAsync(StripePaymentRequestDto request)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = request.Currency,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent = null;

            try
            {
                paymentIntent = await service.CreateAsync(options);
            }
            catch (StripeException ex)
            {
                throw new Exception($"Stripe error: {ex.StripeError.Message}");
            }

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = "Stripe",
                Status = "Pending",
                StripePaymentIntentId = paymentIntent.Id,
                Currency = request.Currency,
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddPaymentAsync(payment);
            return _mapper.Map<PaymentDto>(payment);
        }


        /// <summary>
        /// Create offline/manual payment (optional)
        /// </summary>
        public async Task<PaymentDto> CreatePaymentAsync(PaymentCreateDto dto)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                Status = "Pending",
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddPaymentAsync(payment);
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync()
        {
            var payments = await _paymentRepository.GetPaymentsAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task UpdatePaymentStatusAsync(Guid id, UpdatePaymentDto dto)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment != null)
            {
                payment.Status = dto.Status;
                await _paymentRepository.UpdatePaymentAsync(payment);
            }
        }

        public async Task DeletePaymentAsync(Guid id)
        {
            await _paymentRepository.DeletePaymentAsync(id);
        }
    }
}
