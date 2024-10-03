using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Infrastructure.TempModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

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
        public ActionResult<ClientDto> AddClient([FromBody] ClientUserRequest request)
        {
            var clientDto = _clientService.CreateClient(request.ClientRequest, request.UserRequest);
            return Ok(clientDto);
        }

        [HttpPut("[action]")]
        public ActionResult UpdateClient([FromBody] ClientUserRequest request) 
        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            _clientService.UpdateClient(clientId, request.ClientRequest, request.UserRequest);
            
            return NoContent();
        }
        [HttpGet("[action]")]
        public ActionResult<ClientUserDto> GetClientbyId()
        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _clientService.GetClientById(clientId);

            return Ok(user);
        }
    }
}
