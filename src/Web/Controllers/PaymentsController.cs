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
            // Obtener la membresía seleccionada
            var membership = _membershipService.GetByType(request.Type);

            if (membership == null)
            {
                return NotFound("Membresía no encontrada");
            }
            

            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
    {
        new PreferenceItemRequest
        {
            Title = $"Membresía: {membership.Type}",
            Quantity = 1,
            UnitPrice = (decimal?)membership.Price,  // Usar el precio de la membresía
        },
    },
                // Aquí es donde agregamos la URL de notificación
                NotificationUrl = "https://dc0f-190-194-91-229.ngrok-free.app/api/Payments/mercado-pago-approved"
            };


            var preference = await _mercadoPagoService.CrearPreferenciaPago(preferenceRequest);
            

            return Ok(new { init_point = preference.InitPoint });
        }

        [HttpPost("mercado-pago-approved")]
        public async Task<IActionResult> CrearClientePorPagoAprobado([FromBody] MercadoPagoNotificationRequest request)
        {


            try
            {
                // Validar que la notificación tenga datos
                if (request?.Data?.Id == null)
                {
                    Console.WriteLine("Notificación recibida con datos vacíos");
                    return Ok("Notificación recibida, pero sin datos suficientes");
                }

                // Verificar si ya procesamos este pago


                // Obtener información del pago
                var payment = await _mercadoPagoService.ObtenerPagoPorId(request.Data.Id);
                if (payment == null)
                {
                    // Si no puedes obtener el pago, devuelve un 200 OK para evitar reintentos}
                    Console.WriteLine("no se encontro el pago conj ese Id");
                    return Ok("No se encontró el pago con ese ID");
                }
                // Si el pago es aprobado, procesar el cliente

                if (payment.Status == "approved" && payment.StatusDetail == "accredited")
                {

                    Console.WriteLine($"Pago aprobado, procesando cliente... {payment.Status} {payment.StatusDetail}");

                    return Ok(payment);

                }

                return Ok($"Pago procesado correctamente {payment}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Ok("Error al procesar el pago, pero evitando reintentos");
            }   

        }




        //[HttpPost("update-membership")]
        //public async Task<IActionResult> ActualizarMembresiaCliente([FromBody] MercadoPagoNotificationRequest notification)
        //{
        //    // Obtener el DniClient desde los claims del usuario autenticado
        //    var dniClient = User.Claims.FirstOrDefault(c => c.Type == "DniClient")?.Value;

        //    if (string.IsNullOrEmpty(dniClient))
        //    {
        //        return Unauthorized(new { Error = "No se encontró el DniClient en los claims" });
        //    }

        //    try
        //    {
        //        // Verificar el estado del pago usando el PaymentId de la notificación
        //        var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(notification.PaymentId);

        //        // Solo proceder si el estado del pago es 'completed'
        //        if (paymentStatus == "completed") // Aquí puedes cambiar "completed" por el estado adecuado
        //        {
        //            // Actualizar la membresía del cliente
        //            _clientService.UpdatePago(dniClient);

        //            return Ok(new { Message = "Membresía actualizada con éxito" });
        //        }
        //        else
        //        {
        //            return BadRequest(new { Error = "El estado del pago no es válido para actualizar la membresía" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = ex.Message });
        //    }
        //}


    }
}