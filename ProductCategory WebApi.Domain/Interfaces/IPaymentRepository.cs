using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetPaymentsAsync();
        Task AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Guid id);
    }
}
