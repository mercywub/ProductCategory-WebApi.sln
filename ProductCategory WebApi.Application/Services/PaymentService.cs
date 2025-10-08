using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Application.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        private readonly IGenericRepository<Order> _orderRepo;

        public PaymentService( IPaymentRepository paymentRepo, IGenericRepository<Order> orderRepo, IEmailService emailService, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<Payment> ProcessPaymentAsync(PaymentRequestDto dto)
        {
            // ✅ Map DTO → Payment entity
            var payment = _mapper.Map<Payment>(dto);

            // ✅ Save payment
            await _paymentRepo.AddAsync(payment);
            await _paymentRepo.SaveChangesAsync();

            // ✅ Mark related order as paid
            var order = await _orderRepo.GetByIdAsync(payment.OrderId);
            if (order != null)
            {
               
                await _orderRepo.UpdateAsync(order);
                await _orderRepo.SaveChangesAsync();

                // ✅ Send email notification
                await _emailService.SendOrderPaidNotificationAsync(order);
            }

            return payment;
        }

        public async Task<PaymentDto> CreatePaymentAsync(PaymentCreateDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(dto.OrderId);
            if (order == null) throw new ArgumentException("Order not found");

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                Status = "Completed",
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepo.AddAsync(payment);

            // Optionally: update order status
            order.Status = "Paid";
            await _orderRepo.UpdateAsync(order);

            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByOrderAsync(Guid orderId)
        {
            var payments = await _paymentRepo.GetPaymentsByOrderAsync(orderId);
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status,
                PaymentDate = p.PaymentDate
            }).ToList();
        }


        public async Task<bool> ProcessPaymentAsync(Guid orderId, decimal amount, string method)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return false;

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = method,
                Status = "Paid",
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepo.AddAsync(payment);

            // Mark order as Paid
            order.Status = "Paid";
            await _orderRepo.UpdateAsync(order);

            // Send email notification
            await _emailService.SendOrderConfirmationEmailAsync(order.User.Email, order.Id.ToString(), amount);

            return true;
        }
    }
}
