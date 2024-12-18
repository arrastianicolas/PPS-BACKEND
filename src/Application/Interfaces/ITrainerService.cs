﻿using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITrainerService
    {
        TrainerUserDto GetUserById(int Iduser);
        IEnumerable<TrainerUserDto> GetAllTrainers();
        void Delete(string trainerDni);
        void ChangeStateTrainer(string trainerDni);
        TrainerUserDto CreateTrainer(TrainerRequest trainerRequest, UserRequest userRequest);
    }
}
