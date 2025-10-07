using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Domain.Models
{
    public class Gallary
    {
        public Guid Id { get; set; }

        [Required, StringLength(255)]
        public string Image { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        // Many-to-many
        public ICollection<ProductGallery>? ProductGalleries { get; set; }
    }
}
