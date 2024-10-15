using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] bool? actives)
        {
            if (actives.HasValue)            
                return Ok(actives.Value ? _locationService.GetActives() : _locationService.GetAll());

            return Ok(_locationService.GetAll());
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddLocation([FromBody] LocationRequest locationRequest)
        {
            var locationDto = _locationService.CreateLocation(locationRequest);
            return Ok(locationDto);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateLocation([FromBody] LocationRequest locationRequest, [FromRoute] int id)
        {
            try
            {
                _locationService.UpdateLocation(locationRequest, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteLocation([FromRoute] int id)
        {
            try
            {
                _locationService.DeleteLocation(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("[action]/{locationId}")]
        public IActionResult ChangeState([FromRoute] int locationId)
        {
            try
            {
                _locationService.ChangeState(locationId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
