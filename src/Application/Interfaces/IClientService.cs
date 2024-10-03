using Application.Models;
using Application.Models.Requests;
using Infrastructure.TempModels;
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

        void UpdateClient(int Iduser, ClientRequest clientRequest, UserRequest userRequest);
        ClientUserDto GetClientById(int Iduser);
    }
}
