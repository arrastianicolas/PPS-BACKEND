using Application.Interfaces;
using Application.Models.Requests;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;



namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NutritionalPlanController : ControllerBase
    {
        private readonly INutritionalPlanService _nutritionalPlanService;

        public NutritionalPlanController(INutritionalPlanService nutritionalPlanService)
        {
            _nutritionalPlanService = nutritionalPlanService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var plans = _nutritionalPlanService.GetAll();
            return Ok(plans);
        }

        [HttpGet("[action]")]
        public IActionResult GetMyPlans()
        {
            try
            {
                var userType = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
                return Ok(_nutritionalPlanService.GetByDni(userId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }        

        [HttpPost]
        [Authorize(Roles = "Client")]
        public IActionResult Create([FromBody] NutritionalPlanClientRequest request )
        {
            try
            {
                int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
                var createdPlan = _nutritionalPlanService.Create(clientId, request);
                return Ok(createdPlan);
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
        [Authorize(Roles = "trainer")]
        public IActionResult Update(int id, NutritionalPlanTrainerRequest request)
        {
            try
            {
                _nutritionalPlanService.Update(id, request);
                return NoContent();
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
                _nutritionalPlanService.Delete(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }


    }
}
