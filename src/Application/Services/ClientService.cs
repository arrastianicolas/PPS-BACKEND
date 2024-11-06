using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Exceptions;
using Domain.Interfaces;


using MercadoPago.Resource.User;
using Domain.Entities;
using User = Domain.Entities.User;


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
            if (existingUserWithSameEmail != null) // Cambiado de null a !null
            {
                throw new Exception("Ya existe un usuario con el mismo correo electrónico.");
            }

            // Validación para verificar que no exista otro cliente con el mismo Dniclient
            var existingClientWithSameDni = _clientRepository.GetByDni(clientRequest.Dniclient);
            if (existingClientWithSameDni != null) // Cambiado de null a !null
            {
                throw new Exception("Ya existe un cliente con el mismo DNI.");
            }

            // Obtener el tipo de membresía correspondiente
            var membership = _membershipRepository.Get()
                .FirstOrDefault(m => m.Type == clientRequest.Typememberships);
            if (membership == null)
            {
                throw new Exception("No se encontró la membresía");
            }



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

        public void UpdateClient(int clientId, UpdateClientRequest request)
        {
            // Obtener el cliente por Iduser
            var client = _clientRepository.GetClientByUserId(clientId);
            if (client == null)
            {
                throw new Exception("Client not found.");
            }

            // Obtener el usuario asociado
            var user = _userRepository.GetById(clientId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Validar que el valor no sea una cadena de texto predeterminada ("string" o similar)
            if (!string.IsNullOrEmpty(request.Email) && request.Email != "string")
            {
                user.Email = request.Email;
            }

            // Verificar que el valor no sea una cadena de texto predeterminada (como "string")
            if (!string.IsNullOrEmpty(request.Firstname) && request.Firstname != "string")
            {
                client.Firstname = request.Firstname;
            }

            if (!string.IsNullOrEmpty(request.Lastname) && request.Lastname != "string")
            {
                client.Lastname = request.Lastname;
            }

            if (!string.IsNullOrEmpty(request.Genre) && request.Genre != "string")
            {
                client.Genre = request.Genre;
            }

            // Guardamos los cambios en el cliente y el usuario
            _userRepository.Update(user);  // Actualizamos el usuario
            _clientRepository.Update(client);  // Actualizamos el cliente
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

        public void ChangeStateClient(string clientDni)
        {
            var client = _clientRepository.GetByDni(clientDni) ?? throw new Exception("Client not found.");

            client.Isactive = client.Isactive == 1 ? 0 : 1;

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
        // servicio de updatePago (renovar membresia)
        public void UpdatePago(string dniClient)
        {
            var client = _clientRepository.GetByDni(dniClient);
            client.Isactive = 1; // Activar la membresía
            client.Actualdatemembership = DateTime.Now;

            _clientRepository.Update(client);
        }
        public IEnumerable<object> GetNewClientsCountPerMonth()
        {
            return _clientRepository.GetNewClientsCountPerMonth();
        }

        public IEnumerable<ClientUserDto> GetAllClients()
        {
            var clients = _clientRepository.Get();

            if (clients == null || !clients.Any())
            {
                throw new NotFoundException("No se encontraron clientes.");
            }

            var clientsUserDtos = new List<ClientUserDto>();

            foreach (var client in clients)
            {
                var user = _userRepository.GetById(client.Iduser);

                if (user == null)
                {
                    throw new NotFoundException("Usuario no encontrado");
                }
                var clientUserDto = ClientUserDtoMapper.Create(client, user);
                clientsUserDtos.Add(clientUserDto);
            }

            return clientsUserDtos;
        }
    }

}

