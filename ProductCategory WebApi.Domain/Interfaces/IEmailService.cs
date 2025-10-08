using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string otpCode);
        Task SendOrderConfirmationEmailAsync(string email, string orderNumber, decimal amount);
        Task SendOrderPaidNotificationAsync(Order order);
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
