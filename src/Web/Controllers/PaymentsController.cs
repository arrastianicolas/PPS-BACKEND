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

            // Crear la preferencia de pago con los detalles del producto/membresía
            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
        {
            new PreferenceItemRequest
            {
                Title = $"Membresía: {membership.Type}",
                Quantity = 1,
                UnitPrice = (decimal?)membership.Price, // Precio de la membresía
                //PictureUrl = "https://www.google.com/imgres?q=training%20center&imgurl=https%3A%2F%2Flookaside.fbsbx.com%2Flookaside%2Fcrawler%2Fmedia%2F%3Fmedia_id%3D100063510893314&imgrefurl=https%3A%2F%2Fwww.facebook.com%2Ftrainingcenterrosario%2F%3Flocale%3Des_LA&docid=gXCR07NpMleQnM&tbnid=VdLaKjVvf1bEwM&vet=12ahUKEwjpy5jI0YSJAxXglZUCHfkFJGcQM3oECGUQAA..i&w=1080&h=1080&hcb=2&ved=2ahUKEwjpy5jI0YSJAxXglZUCHfkFJGcQM3oECGUQAA"
            },
        },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://www.instagram.com/miguel_cabrera_3520/",   // URL a la que se redirige cuando el pago es aprobado
                    Failure = "https://mi-sitio.com/pago-fallido",   // URL para pagos fallidos
                    Pending = "https://mi-sitio.com/pago-pendiente"  // URL para pagos pendientes
                },
                AutoReturn = "approved" // Redirige automáticamente a la URL de éxito si el pago es aprobado
            };

            // Llamada al servicio de MercadoPago para crear la preferencia
            var preference = await _mercadoPagoService.CrearPreferenciaPago(preferenceRequest);

            // Retornar el punto de inicialización de pago (init_point) para que el cliente lo use
            return Ok(new { init_point = preference.InitPoint });
        }

    }
}