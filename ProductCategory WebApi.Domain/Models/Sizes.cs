using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
   public class Sizes
    {
        public Guid Id { get; set; }

        [Required, StringLength(50)]
        public string? size { get; set; } 

        // Many-to-many
        public ICollection<ProductSize>? ProductSizes { get; set; }
    }
}
