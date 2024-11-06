using Application.Interfaces;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutinesexerciseController : ControllerBase
    {
        private readonly IRoutinesexerciseService _routinesexerciseService;
        public RoutinesexerciseController(IRoutinesexerciseService routinesexerciseService)
        {
            _routinesexerciseService = routinesexerciseService;
        }
        [HttpGet("[action]")]
        public IActionResult GetMyRoutinesExercise()
        {
            try
            {
                var userType = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
                return Ok(_routinesexerciseService.GetRoutineWithExercises(userId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
