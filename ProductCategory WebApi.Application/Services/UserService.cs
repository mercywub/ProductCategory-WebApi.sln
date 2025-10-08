using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repository, IEmailService emailService, IConfiguration config)
        {
            _repository = repository;
            _emailService = emailService;
            _config = config;
        }

        // Password strength validation
        private bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8) return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }


        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync(); // from GenericRepository
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName
            }).ToList();
        }
        // Register user
        public async Task<UserResponseDto?> RegisterAsync(UserRegisterDto dto)
        {
            if (!IsPasswordStrong(dto.Password))
                throw new ArgumentException("Password must be at least 8 characters, include uppercase, lowercase, number, and special character.");

            var existing = await _repository.GetByEmailAsync(dto.Email);
            if (existing != null) return null;

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _repository.AddAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Token = GenerateJwtToken(user)
            };
        }

        // Login user
        public async Task<UserResponseDto?> LoginAsync(UserLoginDto dto)
        {
            var user = await _repository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Token = GenerateJwtToken(user)
            };
        }

        // Request OTP for password reset
        public async Task GenerateOtpAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null) throw new ArgumentException("User not found.");

            // Generate 4-digit OTP
            var otpCode = new Random().Next(1000, 9999).ToString();

            var otp = new PasswordResetOtp
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                OtpCode = otpCode,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30) // 30-minute validity
            };

            await _repository.AddOtpAsync(otp);

            // Send OTP email
            await _emailService.SendOtpEmailAsync(email, otpCode);
        }

        // Reset password with OTP
        public async Task<bool> ResetPasswordAsync(string email, string otpCode, string newPassword)
        {
            if (!IsPasswordStrong(newPassword))
                throw new ArgumentException("Password must be at least 8 characters, include uppercase, lowercase, number, and special character.");

            var user = await _repository.GetByEmailAsync(email);
            if (user == null) return false;

            var otp = await _repository.GetOtpByUserAsync(user.Id, otpCode);
            if (otp == null || otp.ExpiresAt < DateTime.UtcNow) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _repository.UpdateAsync(user);

            // Delete OTP after use
            await _repository.DeleteOtpAsync(otp);

            return true;
        }

        // JWT generation
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("fullname", user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
