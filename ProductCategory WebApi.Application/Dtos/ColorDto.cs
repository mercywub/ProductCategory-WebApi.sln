using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Application.Dtos
{
    public class ColorDto
    {
        public Guid Id { get; set; }
        public string? Text  { get; set; }
        public string? Color { get; set; }
    }
}
