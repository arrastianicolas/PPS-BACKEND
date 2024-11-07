using Application.Models;
using Application.Models.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClientService
    {
        ClientDto CreateClient(ClientRequest clientRequest, UserRequest userRequest);

        void UpdateClient(int Iduser, UpdateClientRequest request);
        ClientUserDto GetUserById(int Iduser);
        void Delete(string clientDni);
        ClientDto GetClientByDni(string dniClient);
        void UpdatePago(int clientId, string membership);
        IEnumerable<object> GetNewClientsCountPerMonth();

        IEnumerable<ClientUserDto> GetAllClients();

        void ChangeStateClient(string clientDni);

    }
}
