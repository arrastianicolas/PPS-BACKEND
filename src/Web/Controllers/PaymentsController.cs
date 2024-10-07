using Application.Interfaces;
using Application.Models.Requests;
using MercadoPago.Client.Preference;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var membership =  _membershipService.GetByType(request.Type);

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
            }
        }
            };
            
            var preference = await _mercadoPagoService.CrearPreferenciaPago(preferenceRequest);
           
            return Ok(new { init_point = preference.InitPoint });
        }
        [HttpPost("mercado-pago-webhook")]
        public async Task<IActionResult> RecibirNotificacionPago([FromBody] MercadoPagoNotificationRequest notification)
        {
            // El PaymentId debería venir en la notificación que envía Mercado Pago automáticamente
            var paymentId = notification.PaymentId;

            // Verificar el estado del pago en Mercado Pago usando el PaymentId recibido
            var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(paymentId);

            if (paymentStatus == "approved")
            {
                try
                {
                    // Si el pago fue aprobado, puedes proceder con la creación del cliente
                    var newClient = _clientService.CreateClient(notification.ClientRequest, notification.UserRequest);
                    return Ok(new { Message = "Cliente creado con éxito", Cliente = newClient });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message });
                }
            }

            return BadRequest(new { Error = "El pago no fue aprobado" });
        }


        [HttpPost("update-membership")]
        public async Task<IActionResult> ActualizarMembresiaCliente([FromBody] MercadoPagoNotificationRequest notification)
        {
            // Obtener el DniClient desde los claims del usuario autenticado
            var dniClient = User.Claims.FirstOrDefault(c => c.Type == "DniClient")?.Value;

            if (string.IsNullOrEmpty(dniClient))
            {
                return Unauthorized(new { Error = "No se encontró el DniClient en los claims" });
            }

            try
            {
                // Verificar el estado del pago usando el PaymentId de la notificación
                var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(notification.PaymentId);

                // Solo proceder si el estado del pago es 'completed'
                if (paymentStatus == "completed") // Aquí puedes cambiar "completed" por el estado adecuado
                {
                    // Actualizar la membresía del cliente
                    _clientService.UpdatePago(dniClient);

                    return Ok(new { Message = "Membresía actualizada con éxito" });
                }
                else
                {
                    return BadRequest(new { Error = "El estado del pago no es válido para actualizar la membresía" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


    }
}