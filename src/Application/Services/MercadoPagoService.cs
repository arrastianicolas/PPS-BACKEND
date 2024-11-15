using Application.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using MercadoPago.Client.Payment; // Cliente de pagos
using MercadoPago.Resource.Payment; // Recurso de pago
using MercadoPago.Config; // Para configurar el token de MercadoPago
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Application.Services
{
    public class MercadoPagoService : IMercadoPagoService

     {

        private readonly HttpClient _httpClient;

        public MercadoPagoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }
        // Método para crear una preferencia de pago en MercadoPago
        public async Task<Preference> CrearPreferenciaPago(PreferenceRequest preferenceRequest)
         {
                var client = new PreferenceClient();
                var preference = await client.CreateAsync(preferenceRequest);
                return preference;
         }

        public async Task<Payment> ObtenerPagoPorId(string paymentId)
        {
            var response = await _httpClient.GetAsync($"https://api.mercadopago.com/v1/payments/{paymentId}");



            var paymentJson = await response.Content.ReadAsStringAsync();
            var payment = JsonConvert.DeserializeObject<Payment>(paymentJson);

            return payment;
        }
    }
    
}
