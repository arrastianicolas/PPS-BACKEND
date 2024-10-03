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
        Client CreateClient(ClientRequest clientRequest, UserRequest userRequest);
    }
}
