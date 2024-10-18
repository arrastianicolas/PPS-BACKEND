using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Models.Requests;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [HttpPost("[action]/{locationId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddShift([FromBody] int shiftId, [FromRoute] int locationId)
        {
            try
            {
                _shiftService.AddShift(shiftId, locationId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        //[HttpDelete("[action]/{locationId}")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult RemoveShift([FromBody] int shiftId, [FromRoute] int locationId)
        //{
        //    try
        //    {
        //        _shiftService.RemoveShift(shiftId, locationId);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //}
        [HttpPost("[action]")]
        public ActionResult ReserveShift([FromBody] int shiftId) 
        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            try
            {
                _shiftService.ReserveShift(shiftId, clientId);
                return Ok("Turno Reservado Correctamente");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



    }
}