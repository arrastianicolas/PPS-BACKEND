using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Models.Requests;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using System.Security.Claims;
using Application.Models;

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
        [Authorize(Roles = "Client")]
        public ActionResult ReserveShift([FromBody] int shiftId)
        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            try
            {
                _shiftService.ReserveShift(shiftId, clientId);
                return Ok("Turno Reservado Correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<ShiftDto>> AssignTrainerToShift([FromBody] AssignTrainerRequest request)
        {
            try
            {
                var updatedShift = _shiftService.AssignTrainerToShifts(request);
                return Ok(updatedShift);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public ActionResult<List<ShiftDto>> GetShiftsByLocationAndDate([FromQuery] ShiftLocationDayRequest request)
        {
            try
            {
                var shiftDayLocation = _shiftService.GetShiftsByLocationAndDate(request);
                return Ok(shiftDayLocation);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        [HttpGet("[action]")]

        public ActionResult<ShiftMydetailsDto> GetMyShiftDetails()
        {
            int clientId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var shift = _shiftService.GetMyShiftDetails(clientId);
            return Ok(shift);
        }
    }
}