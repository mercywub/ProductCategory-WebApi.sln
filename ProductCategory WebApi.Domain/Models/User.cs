using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? PasswordResetOtp { get; set; }
        public ICollection<PasswordResetOtp>? PasswordResetOtps { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }

        public Cart? Cart { get; set; }
    }
}
