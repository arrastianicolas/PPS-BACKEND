using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ICustomAuthenticationService _customAuthenticationService;

        public AuthenticationController(IConfiguration config, ICustomAuthenticationService autenticacionService)
        {
            _config = config; //Hacemos la inyección para poder usar el appsettings.json
            _customAuthenticationService = autenticacionService;
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <remarks>
        /// Return a JWT token for the user logged in, with a role claim iqual to userType passed in the body.
        /// UserType value must be "SysAdmin", "Client" or "Seller", case sensitive.
        /// </remarks>
        [HttpPost("authenticate")] //Vamos a usar un POST ya que debemos enviar los datos para hacer el login
        public ActionResult<string> Autenticar(AuthenticationRequest authenticationRequest) //Enviamos como parámetro la clase que creamos arriba

        {
            try
            {
                string token = _customAuthenticationService.Autenticar(authenticationRequest);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true,   
                    SameSite = SameSiteMode.None, 
                    Expires = DateTime.UtcNow.AddHours(1) 
                };

                Response.Cookies.Append("jwtToken", token, cookieOptions);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userTypeClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role"); 

                var userType = userTypeClaim?.Value ?? "Unknown"; 

                return Ok(new { message = "Authenticated", 
                    tokenExpires = cookieOptions.Expires, 
                    token,
                    userType
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            var jwtToken = Request.Cookies["jwtToken"];

            if (jwtToken != null)
            {
                Response.Cookies.Append("jwtToken", "", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(-1), 
                    HttpOnly = true,  
                    Secure = true,    
                    SameSite = SameSiteMode.None, 
                    Path = "/",  
                });
            }

            return Ok(new { message = "Cierre de sesión exitoso" });
        }

    }
}
