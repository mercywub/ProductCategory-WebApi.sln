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
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        IUserRepository _userRepository;

        public OrderService(IUserRepository userRepository,ICartRepository cartRepository,IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<OrderResponseDto> CheckoutAsync(CheckoutDto dto)
        {
            // 1️⃣ Get user's cart with items
            var cart = await _cartRepository.GetCartWithItemsAsync(dto.UserId);
            if (cart == null || cart.CartItems.Count == 0)
                throw new ArgumentException("Cart is empty.");

            // 2️⃣ Calculate total
            decimal total = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            // 3️⃣ Create Order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                AddressId = dto.AddressId,
                TotalAmount = total,
                Status = "Pending",
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            // 4️⃣ Clear Cart
            await _cartRepository.ClearCartAsync(cart.Id);

            // 5️⃣ Return response
            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                UserId = dto.UserId,
                AddressId = dto.AddressId
            };
        }
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var created = await _orderRepository.CreateOrderAsync(order);
            return _mapper.Map<OrderDto>(created);
        }

        public async Task UpdateOrderStatusAsync(Guid id, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _orderRepository.UpdateOrderAsync(order);
            }
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
