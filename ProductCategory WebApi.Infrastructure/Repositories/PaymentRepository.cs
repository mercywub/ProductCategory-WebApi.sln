using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using ProductCategory_WebApi.Infrastructure.Datas;

namespace ProductCategory_WebApi.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DBContext _context;

        public PaymentRepository(DBContext context)
        {
            _context = context;
        }

        // Generic Add
        public async Task AddAsync(Payment entity)
        {
            await _context.Payments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Specific Add
        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        // Generic Delete
        public async Task DeleteAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        // Specific Delete
        public async Task DeletePaymentAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        // Generic Get All
        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ToListAsync();
        }

        // Specific Get All
        public async Task<IEnumerable<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ToListAsync();
        }

        // Generic Get by Id
        public async Task<Payment> GetByIdAsync(Guid id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Specific Get by Id
        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Generic Update
        public async Task UpdateAsync(Payment entity)
        {
            _context.Payments.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Specific Update
        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        // SaveChanges (optional, in case you want manual commit)
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
