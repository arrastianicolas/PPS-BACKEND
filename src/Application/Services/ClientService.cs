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
    }
}
