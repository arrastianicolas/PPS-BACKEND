using Application.Interfaces;
using Application.Models.Requests;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll()
        {
            var plans = _nutritionalPlanService.GetAll();
            return Ok(plans);
        }

        [HttpPost]
        public IActionResult Create()
        {
            try
            {
                string trainerDni = "12345678"; // obtener trainer

                string clientDni = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
                var userType = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userType != "Client") return Forbid();

                var createdPlan = _nutritionalPlanService.Create(clientDni, trainerDni);
                return CreatedAtAction(nameof(GetAll), new { id = createdPlan.Id }, createdPlan);
            }            
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, NutritionalPlanRequest request)
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
