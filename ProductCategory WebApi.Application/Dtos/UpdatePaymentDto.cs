using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Application.Dtos
{
    public class UpdatePaymentDto
    {
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
    }
}
