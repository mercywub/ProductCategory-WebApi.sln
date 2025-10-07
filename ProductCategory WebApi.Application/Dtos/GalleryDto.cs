using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Application.Dtos
{
    public class GalleryDto
    {
        public Guid Id { get; set; }
        public string? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
