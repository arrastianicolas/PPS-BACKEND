using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class PaymentRequest
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
    }
}
