using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Infrastructure.TempModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;

        public ClientController(IUserService userService, IClientService clientService)
        {
            _userService = userService;
            _clientService = clientService;
        }

        [HttpPost("[action]")]
        public ActionResult<Client> AddClient([FromBody] ClientUserRequest request)
        {
            var client = _clientService.CreateClient(request.ClientRequest, request.UserRequest);
            return Ok(client);
        }
    }
}
