using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;



namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult Create(NutritionalPlanRequest request)
        {
            var createdPlan = _nutritionalPlanService.Create(request);
            return CreatedAtAction(nameof(GetAll), new { id = createdPlan.Id }, createdPlan);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, NutritionalPlanRequest request)
        {
            _nutritionalPlanService.Update(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _nutritionalPlanService.Delete(id);
            return NoContent();
        }



    }
}
