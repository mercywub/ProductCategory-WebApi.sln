using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
    public class PasswordResetOtp
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; } // FK to User

        [Required]
        [StringLength(6)]
        public string OtpCode { get; set; } = string.Empty; // 6-digit OTP

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiresAt { get; set; } // e.g., CreatedAt + 30 seconds

        // Navigation property (optional)
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
