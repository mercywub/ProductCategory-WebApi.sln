using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using ProductCategory_WebApi.Infrastructure.Datas;

namespace ProductCategory_WebApi.Infrastructure.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly DBContext _context;
        public AddressRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Address existing)
        {
            _context.Addresses.Remove(existing); // Remove the entity from DbSet
            await _context.SaveChangesAsync();   // Save changes to the database
        }

        public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
