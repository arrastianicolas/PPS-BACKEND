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
        public PaymentsController(IMercadoPagoService mercadoPagoService, IMembershipService membershipService)
        {
            _mercadoPagoService = mercadoPagoService;
            _membershipService = membershipService; 
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
        //[HttpPost("mercado-pago-notification")]
        //public async Task<IActionResult> MercadoPagoNotification([FromBody] MercadoPagoNotificationRequest notification)
        //{
        //    // Verificar el estado del pago desde la notificación
        //    var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(notification.PaymentId);

        //    if (paymentStatus == "approved")
        //    {
        //        // Actualizar el estado de la membresía del cliente a "Activa"
        //        var client = await _clientRepository.GetById(notification.ClientId);
        //        client.Isactive = 1; // Activar la membresía
        //        client.Startdatemembership = DateTime.Now;
        //        client.Actualdatemembership = DateTime.Now;

        //        await _clientRepository.Update(client);
        //    }

        //    return Ok();
        //}
    }
}