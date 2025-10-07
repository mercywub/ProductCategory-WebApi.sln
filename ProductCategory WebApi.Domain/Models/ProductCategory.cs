using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
   public  class ProductCategory
    {
        public Guid Id { get; set; }
        public String? CategoryName { get; set; } 
        public String? CategoryDescription { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
