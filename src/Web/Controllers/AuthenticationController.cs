using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            //Lo primero que hacemos es llamar a una función que valide los parámetros que enviamos.
            try
            {
                string token = _customAuthenticationService.Autenticar(authenticationRequest);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // La cookie no es accesible desde JavaScript
                    Secure = true,   // Solo se envía en conexiones HTTPS
                    SameSite = SameSiteMode.Lax, // Previene ataques CSRF
                    Expires = DateTime.UtcNow.AddHours(1) // La cookie expira en 1 hora
                };

                Response.Cookies.Append("jwtToken", token, cookieOptions);

                // Devolvemos una respuesta exitosa
                return Ok(new { message = "Authenticated", tokenExpires = cookieOptions.Expires, token
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
