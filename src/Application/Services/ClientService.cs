using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Interfaces;
using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMembershipRepository _membershipRepository;

        public ClientService(IUserRepository userRepository, IClientRepository clientRepository, IMembershipRepository membershipRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _membershipRepository = membershipRepository;
        }

        public Client CreateClient(ClientRequest clientRequest, UserRequest userRequest)
        {
            // Verifica si el tipo de membresía existe en la base de datos
            var membership = _membershipRepository.Get()
                .FirstOrDefault(m => m.Type == clientRequest.Typememberships);

            if (membership == null)
            {
                throw new Exception("El tipo de membresía no existe.");
            }

            // Crea el usuario
            var user = new User
            {
                Email = userRequest.Email!,
                Password = userRequest.Password!,
                Type = "Client"
            };

            var createdUser = _userRepository.Add(user);

            // Crea el cliente
            var client = new Client
            {
                Dniclient = clientRequest.Dniclient,
                Firstname = clientRequest.Firstname,
                Lastname = clientRequest.Lastname,
                Birthdate = clientRequest.Birthdate,
                Phonenumber = clientRequest.Phonenumber,
                Typememberships = membership.Type, // Asigna el tipo de membresía validado
                Startdatemembership = DateTime.Now,
                Statusmembership = "Activa",
                Iduser = createdUser.Id // Relaciona el usuario creado con el cliente
            };

            return _clientRepository.Add(client);
        }
    }
}
