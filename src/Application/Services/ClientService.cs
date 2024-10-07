using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain;
using Infrastructure.TempModels;
using MercadoPago.Resource.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using User = Infrastructure.TempModels.User;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMailService _mailService;

        public ClientService(IUserRepository userRepository, IClientRepository clientRepository, IMembershipRepository membershipRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _membershipRepository = membershipRepository;
            _mailService = mailService;
        }

        public ClientDto CreateClient(ClientRequest clientRequest, UserRequest userRequest)
        {
            // Validación para verificar que no exista otro usuario con el mismo email
            var existingUserWithSameEmail = _userRepository.GetByUserEmail(userRequest.Email);

            if (existingUserWithSameEmail != null)
            {
                throw new Exception("Ya existe un usuario con el mismo correo electrónico.");
            }

            // Validación para verificar que no exista otro cliente con el mismo Dniclient
            var existingClientWithSameDni = _clientRepository.GetByDni(clientRequest.Dniclient);

            if (existingClientWithSameDni != null)
            {
                throw new Exception("Ya existe un cliente con el mismo DNI.");
            }

            // Obtener el tipo de membresía correspondiente
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
                Isactive = 1,
                Startdatemembership = DateTime.Now,
                Actualdatemembership = DateTime.Now,
                Genre = clientRequest.Genre,
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
                throw new NotFoundException($"No se encontró un usuario con el ID: {Iduser}");
            }
            var existingUserWithSameEmail = _userRepository.GetByUserEmail(userRequest.Email);

            if (existingUserWithSameEmail != null)
            {
                throw new Exception("Ya existe un usuario con el mismo correo electrónico.");
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
        public ClientUserDto GetUserById(int Iduser)
        {
            
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }

            
            var clientUser = _clientRepository.GetClientByUserId(Iduser);
            if (clientUser == null)
            {
                throw new NotFoundException("No se encontro al cliente.");
            }

            // Retorna el DTO combinado de usuario y cliente
            return ClientUserDtoMapper.Create(clientUser, user);
        }

        public void Delete(string clientDni)
        {
            var client = _clientRepository.GetByDni(clientDni);
            if (client == null)
            {
                throw new NotFoundException("No se encontro al cliente.");
            }
            client.Isactive = 0;
            _clientRepository.Update(client);
            
        }

        public ClientDto GetClientByDni(string dniClient)
        {

      

            var clientUser = _clientRepository.GetByDni(dniClient);
            if (clientUser == null)
            {
                throw new NotFoundException("No se encontro al cliente.");
            }

            // Retorna el DTO combinado de usuario y cliente
            return ClientDto.Create(clientUser);
        }

        public void UpdatePago(string dniClient)
        {
            var client = _clientRepository.GetByDni(dniClient);
            client.Isactive = 1; // Activar la membresía
            client.Startdatemembership = DateTime.Now;
            client.Actualdatemembership = DateTime.Now;

            _clientRepository.Update(client);
        }

    }
}
