using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

       [HttpGet("[action]")]
        public ActionResult<TrainerUserDto> GetMyDetails()
        {
            int trainerId = int.Parse(User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _trainerService.GetUserById(trainerId);

            return Ok(user);
        }

        [HttpGet("[action]")]
        public IActionResult GetAllTrainers()
        {
            try
            {
                var trainers = _trainerService.GetAllTrainers();
                return Ok(trainers);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public IActionResult AddTrainer([FromBody] TrainerUserRequest request)
        {
            try
            {
                var trainerUserDto = _trainerService.CreateTrainer(request.TrainerRequest, request.UserRequest);
                return Ok(trainerUserDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPut("[action]/{trainerDni}")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeState([FromRoute] string trainerDni)
        {
            try
            {
                _trainerService.ChangeStateTrainer(trainerDni);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(string trainerDni)
        {
            try
            {
                _trainerService.Delete(trainerDni);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
