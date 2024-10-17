using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        private readonly IMemoryCache _memoryCache;

        public UserController(IUserService sysAdminService, IMemoryCache memoryCache)
        {
            _UserService = sysAdminService;
            _memoryCache = memoryCache;
        }


        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(int id)
        {
            try
            {
                var user = _UserService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<List<UserDto>> Get()
        {
            try
            {
                var userAll = _UserService.Get();
                return Ok(userAll);
            } 
             catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public IActionResult RequestResetPassword([FromBody] string email)
        {
            try
            {
                var code = _UserService.RequestResetPassword(email);
                _memoryCache.Set(email, code, TimeSpan.FromMinutes(15));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var cachedCode = _memoryCache.Get<string>(request.Email);
            Console.WriteLine($"Cached: {cachedCode}");
            Console.WriteLine($"Request: {request.Code}");
            if (cachedCode == null || cachedCode != request.Code)
                return BadRequest("Código inválido o expirado.");

            try
            {
                _UserService.ResetPassword(request);
                _memoryCache.Remove(request.Email);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
