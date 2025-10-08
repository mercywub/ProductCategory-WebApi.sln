using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Domain.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task DeleteAsync(Address existing);
        Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);
    }
}
