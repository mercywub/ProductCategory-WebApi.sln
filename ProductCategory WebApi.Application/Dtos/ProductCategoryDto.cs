using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Application.Dtos
{
    public class ProductCategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
    }
}
