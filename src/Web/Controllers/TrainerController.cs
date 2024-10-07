using Application.Interfaces;
using Application.Models;
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
    }
}
