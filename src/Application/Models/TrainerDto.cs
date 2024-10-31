using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TrainerDto
    {
        public string DniTrainer { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public int Isactive { get; set; }

        // Método estático para mapear de Trainer a TrainerDto
        public static TrainerDto Create(Trainer trainer)
        {
            return new TrainerDto
            {
                DniTrainer = trainer.Dnitrainer,
                FirstName = trainer.Firstname,
                LastName = trainer.Lastname,
                BirthDate = trainer.Birthdate,
                PhoneNumber = trainer.Phonenumber,
                Isactive = trainer.Isactive,
            };
        }
    }
}
