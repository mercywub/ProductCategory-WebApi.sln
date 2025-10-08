using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        

        [Required]
        public string PaymentMethod { get; set; } = "CreditCard"; // or PayPal, CashOnDelivery, etc.

        [Required]
        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
        [StringLength(100)]
        public string StripePaymentIntentId { get; set; } // Stripe PaymentIntent ID

        [StringLength(10)]
        public string Currency { get; set; } = "USD";

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string TransactionId { get; set; } 
    }
}
