using Microsoft.EntityFrameworkCore;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using ProductCategory_WebApi.Infrastructure.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DBContext _context;

        public UserRepository(DBContext context) : base(context)
        {
            _context = context;
        }

    

        // OTP handling methods
        public async Task AddOtpAsync(PasswordResetOtp otp)
        {
            await _context.PasswordResetOtps.AddAsync(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetOtp?> GetOtpByUserAsync(Guid userId, string otpCode)
        {
            return await _context.PasswordResetOtps
                .Where(o => o.UserId == userId && o.OtpCode == otpCode)
                .OrderByDescending(o => o.CreatedAt) // get the latest OTP
                .FirstOrDefaultAsync();
        }

        public async Task DeleteOtpAsync(PasswordResetOtp otp)
        {
            _context.PasswordResetOtps.Remove(otp);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<Cart?> GetCartWithItemsAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == cartId);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
