using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Application.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool Feature { get; set; }
        public string ProductImageFeatured { get; set; }
        public string? Status { get; set; }
        public Guid ProductCategoryId { get; set; }


    }
}
