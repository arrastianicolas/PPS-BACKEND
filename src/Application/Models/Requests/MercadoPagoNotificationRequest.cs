using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class MercadoPagoNotificationRequest
    {
        public string PaymentId { get; set; }
        public ClientRequest ClientRequest { get; set; }
        public UserRequest UserRequest { get; set; }    
    }
}
