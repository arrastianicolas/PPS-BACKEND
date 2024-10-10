using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class MercadoPagoNotificationRequest
    {
        public MercadoPagoData Data { get; set; } 
        
    }

    public class MercadoPagoData
    {
        public string Id { get; set; } = null!; // Este es el paymentId
    }
}
