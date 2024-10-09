using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IMembershipService _membershipService;
        private readonly IClientService _clientService;
       
        public PaymentsController(IMercadoPagoService mercadoPagoService, IMembershipService membershipService, IClientService clientService)
        {
            _mercadoPagoService = mercadoPagoService;
            _membershipService = membershipService;
            _clientService = clientService;
            

        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CrearPreferenciaPago([FromBody] PaymentRequest request)
        {
            // Obtener la membres�a seleccionada
            var membership = _membershipService.GetByType(request.Type);

            if (membership == null)
            {
                return NotFound("Membres�a no encontrada");
            }
            

            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
    {
        new PreferenceItemRequest
        {
            Title = $"Membres�a: {membership.Type}",
            Quantity = 1,
            UnitPrice = (decimal?)membership.Price,  // Usar el precio de la membres�a
        },
    },
                // Aqu� es donde agregamos la URL de notificaci�n
                NotificationUrl = "https://c528-190-194-91-229.ngrok-free.app/api/Payments/mercado-pago-approved"
            };


            var preference = await _mercadoPagoService.CrearPreferenciaPago(preferenceRequest);
            

            return Ok(new { init_point = preference.InitPoint });
        }

        [HttpPost("mercado-pago-approved")]
        public async Task<IActionResult> CrearClientePorPagoAprobado([FromBody] MercadoPagoNotificationRequest request)
        {


            try
            {
                var jsonString = JsonConvert.SerializeObject(request);
                Console.WriteLine("Notificaci�n recibida: " + jsonString);
                var payment = await _mercadoPagoService.ObtenerPagoPorId(request.Data.Id);
                if (payment.Status == "approved")
                {

                    Console.WriteLine("EL PAGO FUE EXITOSO");
                   
                }
                // Maneja el pago aprobado
                return Ok();
            }
            catch (Exception ex)
            {
                // Registrar el error y evitar bucles
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest("Error al procesar el pago.");
            }

        }




        //[HttpPost("update-membership")]
        //public async Task<IActionResult> ActualizarMembresiaCliente([FromBody] MercadoPagoNotificationRequest notification)
        //{
        //    // Obtener el DniClient desde los claims del usuario autenticado
        //    var dniClient = User.Claims.FirstOrDefault(c => c.Type == "DniClient")?.Value;

        //    if (string.IsNullOrEmpty(dniClient))
        //    {
        //        return Unauthorized(new { Error = "No se encontr� el DniClient en los claims" });
        //    }

        //    try
        //    {
        //        // Verificar el estado del pago usando el PaymentId de la notificaci�n
        //        var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(notification.PaymentId);

        //        // Solo proceder si el estado del pago es 'completed'
        //        if (paymentStatus == "completed") // Aqu� puedes cambiar "completed" por el estado adecuado
        //        {
        //            // Actualizar la membres�a del cliente
        //            _clientService.UpdatePago(dniClient);

        //            return Ok(new { Message = "Membres�a actualizada con �xito" });
        //        }
        //        else
        //        {
        //            return BadRequest(new { Error = "El estado del pago no es v�lido para actualizar la membres�a" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = ex.Message });
        //    }
        //}


    }
}