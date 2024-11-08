using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Models.Requests;
using Application.Interfaces;

using System.Security.Claims;
using Application.Models;
using Domain.Exceptions;
using Application.Services;

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
        public ActionResult<RoutineDto> Create([FromBody] RoutineClientRequest routineClientRequest)

        {
            try
            {
                int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
                var routineDto = _routineService.Create(routineClientRequest, clientId);
            return Ok(routineDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotAllowedException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Trainer")]
        public IActionResult Update([FromRoute] int id, [FromBody] List<RoutineTrainerRequest> routineTrainerRequest)
        {
            try
            {
                _routineService.Update(id, routineTrainerRequest);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch]
        [Authorize(Roles = "Trainer")]
        public IActionResult ChangeStatusToDone([FromBody] string dniClient)
        {
            try
            {
                _routineService.ChangeStatusToDone(dniClient);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var routines = _routineService.GetAll();
            return Ok(routines);
        }

        [HttpGet("[action]")]
        public IActionResult GetMyRoutines()
        {
            try
            {
                var userType = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
                return Ok(_routineService.GetByDni(userId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "trainer")]
        public IActionResult Delete(int id)
        {
            try
            {
                _routineService.Delete(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
