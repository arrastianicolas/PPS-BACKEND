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

        public ClientDto CreateClient(ClientRequest clientRequest, UserRequest userRequest)
        {
            var membership = _membershipRepository.Get()
                .FirstOrDefault(m => m.Type == clientRequest.Typememberships);

            var user = new User
            {
                Email = userRequest.Email!,
                Password = userRequest.Password!,
                Type = "Client"
            };

            var createdUser = _userRepository.Add(user);

            var client = new Client
            {
                Dniclient = clientRequest.Dniclient,
                Firstname = clientRequest.Firstname,
                Lastname = clientRequest.Lastname,
                Birthdate = clientRequest.Birthdate,
                Phonenumber = clientRequest.Phonenumber,
                Typememberships = membership.Type,
                Startdatemembership = DateTime.Now,
                Statusmembership = "Activa",
                Iduser = createdUser.Id
            };

            _clientRepository.Add(client);

            return ClientDto.Create(client);
        }
        public void UpdateClient(int Iduser, ClientRequest clientRequest, UserRequest userRequest)
        {
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new KeyNotFoundException($"No se encontró un usuario con el ID: {Iduser}");
            }

            user.Password = userRequest.Password;
            user.Email = userRequest.Email;

            var client = _clientRepository.GetClientByUserId(user.Id);
            if (client != null)
            {
                client.Firstname = clientRequest.Firstname;
                client.Lastname = clientRequest.Lastname;
                client.Phonenumber = clientRequest.Phonenumber;
                client.Birthdate = clientRequest.Birthdate;
            }

            _userRepository.Update(user);
            if (client != null)
            {
                _clientRepository.Update(client);
            }

        }
        public ClientUserDto GetClientById(int Iduser)
        {
            
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            
            var clientUser = _clientRepository.GetClientByUserId(Iduser);
            if (clientUser == null)
            {
                throw new Exception("Cliente no encontrado para el usuario especificado");
            }

            // Retorna el DTO combinado de usuario y cliente
            return ClientUserDtoMapper.Create(clientUser, user);
        }


    }
}
