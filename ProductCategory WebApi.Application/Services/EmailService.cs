using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using Microsoft.Extensions.Hosting;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ProductCategory_WebApi.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IConfiguration _config;
       

        public EmailService(IConfiguration config,IOptions<EmailSettings> options)
        {
            _settings = options.Value;
            _config = config;
            
        }

        public async Task SendOtpEmailAsync(string email, string otpCode)
        {
            string body = $"<p>Your OTP code is <strong>{otpCode}</strong>.</p><p>It expires in 30 minutes.</p>";
            await SendEmailAsync(email, "Password Reset OTP", body);
        }


        public async Task SendOrderConfirmationEmailAsync(string email, string orderNumber, decimal amount)
        {
            var smtp = _config.GetSection("EmailSettings");
            var client = new SmtpClient(smtp["SmtpServer"], int.Parse(smtp["Port"]))
            {
                Credentials = new NetworkCredential(smtp["SenderEmail"], smtp["Password"]),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(smtp["SenderEmail"], smtp["SenderName"]),
                Subject = "Order Confirmation",
                Body = $"Thank you for your purchase!\n\nOrder Number: {orderNumber}\nAmount Paid: ${amount:F2}\n\nYour order is being processed.",
                IsBodyHtml = false
            };

            mail.To.Add(email);
            await client.SendMailAsync(mail);
        }

        public Task SendOrderPaidNotificationAsync(Order order)
        {
            throw new NotImplementedException();
        }

        
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSettings = _config.GetSection("SmtpSettings");
            string fromEmail = smtpSettings["From"];
            string password = smtpSettings["Password"];
            string host = smtpSettings["Host"];
            int port = int.Parse(smtpSettings["Port"]);

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(fromEmail, password);
                client.EnableSsl = true;

                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail, "E-Commerce App"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(toEmail);

                await client.SendMailAsync(mail);
            }
        }

    }
}
