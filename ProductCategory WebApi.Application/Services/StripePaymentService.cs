using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using Stripe;

namespace ProductCategory_WebApi.Application.Services
{

    public class StripePaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IEmailService _emailService;

        public StripePaymentService(
            IConfiguration config,
            IPaymentRepository paymentRepo,
           IGenericRepository<Order> orderRepo,
            IEmailService emailService)
        {
            _config = config;
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _emailService = emailService;
        }

        public async Task<Payment> ProcessPaymentAsync(PaymentRequestDto dto)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

            var order = await _orderRepo.GetByIdAsync(dto.OrderId);
            if (order == null) throw new ArgumentException("Order not found.");

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(dto.Amount * 100), 
                PaymentMethodTypes = new List<string> { dto.PaymentMethod },
               
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            // Save Payment record
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = dto.PaymentMethod,
                Status = "Paid",
                TransactionId = intent.Id
            };

            await _paymentRepo.AddAsync(payment);

            // Mark order as Paid
            order.Status = "Paid";
            await _orderRepo.UpdateAsync(order);

            // Send payment confirmation email
            await _emailService.SendOrderPaidNotificationAsync(order);

            return payment;
        }
    }
}
