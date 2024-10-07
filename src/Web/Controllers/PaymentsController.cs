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
            // Obtener la membres�a seleccionada
            var membership =  _membershipService.GetByType(request.Type);

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
            }
        }
            };
            
            var preference = await _mercadoPagoService.CrearPreferenciaPago(preferenceRequest);
           
            return Ok(new { init_point = preference.InitPoint });
        }
        [HttpPost("mercado-pago-webhook")]
        public async Task<IActionResult> RecibirNotificacionPago([FromBody] MercadoPagoNotificationRequest notification)
        {
            // El PaymentId deber�a venir en la notificaci�n que env�a Mercado Pago autom�ticamente
            var paymentId = notification.PaymentId;

            // Verificar el estado del pago en Mercado Pago usando el PaymentId recibido
            var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(paymentId);

            if (paymentStatus == "approved")
            {
                try
                {
                    // Si el pago fue aprobado, puedes proceder con la creaci�n del cliente
                    var newClient = _clientService.CreateClient(notification.ClientRequest, notification.UserRequest);
                    return Ok(new { Message = "Cliente creado con �xito", Cliente = newClient });
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
                return Unauthorized(new { Error = "No se encontr� el DniClient en los claims" });
            }

            try
            {
                // Verificar el estado del pago usando el PaymentId de la notificaci�n
                var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(notification.PaymentId);

                // Solo proceder si el estado del pago es 'completed'
                if (paymentStatus == "completed") // Aqu� puedes cambiar "completed" por el estado adecuado
                {
                    // Actualizar la membres�a del cliente
                    _clientService.UpdatePago(dniClient);

                    return Ok(new { Message = "Membres�a actualizada con �xito" });
                }
                else
                {
                    return BadRequest(new { Error = "El estado del pago no es v�lido para actualizar la membres�a" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


    }
}