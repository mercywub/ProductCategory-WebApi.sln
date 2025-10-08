using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCategory_WebApi.Application.Dtos
{

    public class CheckoutDto
    {
        public Guid UserId { get; set; } // The user performing checkout
        public Guid AddressId { get; set; } // Selected shipping address
        public string PaymentMethod { get; set; } = "COD"; // Optional
    }
}
