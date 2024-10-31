using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Models.Requests;
using Application.Interfaces;

using System.Security.Claims;
using Application.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoutineController : ControllerBase
    {
        private readonly IRoutineService _routineService;
        public RoutineController(IRoutineService routineService)
        {
            _routineService = routineService;
        }

        //[HttpPut]
        //public ActionResult<Routine> Update(RoutineTrainerRequest routineTrainerRequest)
        //{
        //    return Ok(_routineService.Update());
        //}

        [HttpPost]
        [Authorize(Roles = "Client")]
        public ActionResult<RoutineDto> Add([FromBody] RoutineClientRequest routineClientRequest)

        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            

            var routineDto = _routineService.Add(routineClientRequest, clientId);
            return Ok(routineDto);
        }
    }
}
