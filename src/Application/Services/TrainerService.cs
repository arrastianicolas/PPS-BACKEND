using Application.Interfaces;
using Application.Models;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITrainerRepository _trainerRepository;
        private readonly IMailService _mailService;

        public TrainerService(IUserRepository userRepository, ITrainerRepository trainerRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _trainerRepository = trainerRepository;
            _mailService = mailService;
        }

        public TrainerUserDto GetUserById(int Iduser)
        {

            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }


            var trainerUser = _trainerRepository.GetTrainerByUserId(Iduser);
            if (trainerUser == null)
            {
                throw new NotFoundException("No se encontro al entrenador.");
            }

            // Retorna el DTO combinado de usuario y trainer
            return TrainerUserDtoMapper.Create(trainerUser, user);
        }

        public IEnumerable<TrainerUserDto> GetAllTrainers()
        {
            var trainers = _trainerRepository.Get();

            if (trainers == null || !trainers.Any())
            {
                throw new NotFoundException("No se encontraron entrenadores.");
            }

            var trainerUserDtos = new List<TrainerUserDto>();

            foreach (var trainer in trainers)
            {
                var user = _userRepository.GetById(trainer.Iduser);

                if (user == null)
                {
                    throw new NotFoundException("Usuario no encontrado");
                }
                var trainerUserDto = TrainerUserDtoMapper.Create(trainer, user);
                trainerUserDtos.Add(trainerUserDto);
            }

            return trainerUserDtos;
        }

        public void Delete(string trainerDni)
        {
            var trainer = _trainerRepository.GetByDni(trainerDni);
            if (trainer == null)
            {
                throw new NotFoundException("No se encontro al trainer.");
            }
            trainer.Isactive = 0;
            _trainerRepository.Update(trainer);

        }

        public void ChangeStateTrainer(string trainerDni)
        {
            var trainer = _trainerRepository.GetByDni(trainerDni) ?? throw new Exception("Client not found.");

            trainer.Isactive = trainer.Isactive == 1 ? 0 : 1;

            _trainerRepository.Update(trainer);
        }

    }
}
