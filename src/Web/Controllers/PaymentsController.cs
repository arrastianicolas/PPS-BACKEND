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
        //[HttpPost("mercado-pago-notification")]
        //public async Task<IActionResult> MercadoPagoNotification([FromBody] MercadoPagoNotificationRequest notification)
        //{
        //    // Verificar el estado del pago desde la notificaci�n
        //    var paymentStatus = await _mercadoPagoService.VerificarEstadoPago(notification.PaymentId);

        //    if (paymentStatus == "approved")
        //    {
        //        // Actualizar el estado de la membres�a del cliente a "Activa"
        //        var client = await _clientRepository.GetById(notification.ClientId);
        //        client.Isactive = 1; // Activar la membres�a
        //        client.Startdatemembership = DateTime.Now;
        //        client.Actualdatemembership = DateTime.Now;

        //        await _clientRepository.Update(client);
        //    }

        //    return Ok();
        //}
    }
}