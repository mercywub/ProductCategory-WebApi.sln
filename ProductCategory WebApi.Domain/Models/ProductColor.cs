using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
    public class ProductColor
    {
        public Guid ColorId { get; set; }
        public Colors? Colors { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
