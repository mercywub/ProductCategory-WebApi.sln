using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool Feature { get; set; }
        public string? ProductImageFeatured { get; set; }
        public string? Status { get; set; }


        public Guid ProductCategoryId { get; set; }     // ✅ FK
        public ProductCategory ProductCategory { get; set; } = null!;

        public ICollection<ProductColor>? ProductColors { get; set; }
        public ICollection<ProductSize>? ProductSizes { get; set; }
        public ICollection<ProductGallery>? ProductGalleries { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }

}
