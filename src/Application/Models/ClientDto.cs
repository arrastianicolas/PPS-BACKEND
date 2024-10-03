﻿//using Infrastructure.TempModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using Infrastructure.TempModels;

namespace Application.Models
{
    public class ClientDto
    {
        public string DniClient { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string TypeMembership { get; set; } = null!;
        public DateTime StartDateMembership { get; set; }
        public string StatusMembership { get; set; } = null!;

        // Método estático para mapear de Client a ClientDto
        public static ClientDto Create(Client client)
        {
            return new ClientDto
            {
                DniClient = client.Dniclient,
                FirstName = client.Firstname,
                LastName = client.Lastname,
                BirthDate = client.Birthdate,
                PhoneNumber = client.Phonenumber,
                TypeMembership = client.Typememberships,
                StartDateMembership = client.Startdatemembership,
                StatusMembership = client.Statusmembership
            };

        }
    }
}
