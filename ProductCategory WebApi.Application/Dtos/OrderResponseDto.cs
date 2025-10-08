using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Application.Dtos
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
       
        public string Status { get; set; } = "Pending";
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
    }
}
