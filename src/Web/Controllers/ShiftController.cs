using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Models.Requests;
using Application.Interfaces;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            return Ok(_shiftService.GetAll());
        }

        [HttpPost]
        public IActionResult AddShift([FromBody] ShiftRequest shiftRequest)
        {
            try
            {
                var shiftDto = _shiftService.CreateShift(shiftRequest);
                return Ok(shiftDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShift([FromRoute] int id, [FromBody] ShiftRequest shiftRequest)
        {
            try
            {
                _shiftService.UpdateShift(id, shiftRequest);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
