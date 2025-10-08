using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
       

        Task<User?> GetByEmailAsync(string email);
        Task AddOtpAsync(PasswordResetOtp otp);
        Task<PasswordResetOtp?> GetOtpByUserAsync(Guid userId, string otpCode);
        Task DeleteOtpAsync(PasswordResetOtp otp);
        Task<Cart?> GetCartWithItemsAsync(Guid userId);
        Task ClearCartAsync(Guid cartId);
    }
}
