using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class PaymentRequest
    {
        public string Type { get; set; }

        public BackUrlsRequest BackUrls { get; set; }
    }
    public class BackUrlsRequest
    {
        public string Success { get; set; }
        public string Failure { get; set; }
        public string Pending { get; set; }
    }
}
