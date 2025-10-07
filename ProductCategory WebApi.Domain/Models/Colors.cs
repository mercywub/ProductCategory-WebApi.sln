using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
    public class Colors
    {
        public  Guid Id { get; set; }

        [Required, StringLength(50)]
        public string? Text { get; set; } 

        [StringLength(10)]
        public string? Color { get; set; } // HEX color code

        // Many-to-many
        public ICollection<ProductColor>? ProductColors { get; set; }
    }
}
