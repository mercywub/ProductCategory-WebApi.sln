using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Domain.Interfaces
{
    public interface ICartRepository:IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(Guid userId);
        Task<Cart?> GetCartWithItemsAsync(Guid userId);
        Task ClearCartAsync(Guid cartId);
    }
}
