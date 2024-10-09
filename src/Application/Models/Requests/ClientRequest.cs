
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ClientRequest
    {
        public string Dniclient { get; set; } = null!;

        public string Typememberships { get; set; } = null!;

        public DateTime Startdatemembership { get; set; }

        public DateOnly Birthdate { get; set; }

        public string Phonenumber { get; set; } = null!;

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;
        public string Genre {  get; set; } = null!;
    }
}
