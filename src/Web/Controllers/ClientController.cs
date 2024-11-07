using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Domain.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Client")]
    public class ClientController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;

        public ClientController(IUserService userService, IClientService clientService)
        {
            _userService = userService;
            _clientService = clientService;
        }

        private static ClientUserRequest tempClientData;

        [HttpPost("[action]")]
        public ActionResult<string> StoreClientUserData([FromBody] ClientUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientRequest.Dniclient))
            {
                return BadRequest();
            }

            tempClientData = request;

            return Ok(request.ClientRequest.Dniclient); 
        }

        [HttpGet("[action]")]
        public ActionResult<ClientUserRequest> GetStoredClientUserData()
        {
            if (tempClientData == null)
            {
                return NotFound();
            }

            return Ok(tempClientData); 
        }


        [HttpPost("[action]")]
        public ActionResult<ClientDto> AddClient([FromBody] string clientDni)
        {
            if (tempClientData == null)
            {
                return NotFound();
            }

            if (tempClientData.ClientRequest.Dniclient != clientDni)
            {
                return BadRequest();
            }

            var clientDto = _clientService.CreateClient(tempClientData.ClientRequest, tempClientData.UserRequest);

            tempClientData = null; 

            return Ok(clientDto);
        }


        [HttpPut("[action]")]
        public ActionResult UpdateClient([FromBody] UpdateClientRequest request)
        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (clientId == 0)
            {
                return BadRequest("Client ID is missing or invalid.");
            }

            try
            {
                _clientService.UpdateClient(clientId, request);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]")]
       
        public ActionResult<ClientUserDto> GetMyDetails()
        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _clientService.GetUserById(clientId);

            return Ok(user);
        }
        [HttpDelete("[action]")]
        public IActionResult Delete(string clientDni)
        {
            try
            {
                _clientService.Delete(clientDni);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
           
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<object>> GetNewClientsPerMonth()
        {
            try
            {
                var result = _clientService.GetNewClientsCountPerMonth();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _clientService.GetAllClients();
                return Ok(clients);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("[action]/{clientDni}")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeState([FromRoute] string clientDni)
        {
            try
            {
                _clientService.ChangeStateClient(clientDni);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("[action]")]
        [Authorize]
        public IActionResult UpdatePago([FromBody] string membership)
        {
            // Obtiene el clientId desde las claims del usuario autenticado
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            if (clientId == null)
            {
                return Unauthorized("No se pudo encontrar al cliente");
            }

            _clientService.UpdatePago(clientId, membership);

            return NoContent();
        }
    }
}
