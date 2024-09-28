using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _sysAdminService;

        public UserController(IUserService sysAdminService)
        {
            _sysAdminService = sysAdminService;
        }


        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(int id)
        {
            try
            {
                var user =  _sysAdminService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<List<UserDto>> GetAllUsers()
        {
            var userAll = _sysAdminService.GetAllUsers();
            return Ok(userAll);
        }
    }
}
