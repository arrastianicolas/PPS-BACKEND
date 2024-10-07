using Application.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using MercadoPago.Client.Payment; // Cliente de pagos
using MercadoPago.Resource.Payment; // Recurso de pago
using MercadoPago.Config; // Para configurar el token de MercadoPago
using System.Threading.Tasks;
using System.Configuration;

namespace Application.Services
{
    public class MercadoPagoService : IMercadoPagoService
     {
            // Método para crear una preferencia de pago en MercadoPago
         public async Task<Preference> CrearPreferenciaPago(PreferenceRequest preferenceRequest)
         {
                var client = new PreferenceClient();
                var preference = await client.CreateAsync(preferenceRequest);
                return preference;
         }

            // Método para verificar el estado de un pago en MercadoPago
        public async Task<string> VerificarEstadoPago(string paymentId)
         {
            if (!long.TryParse(paymentId, out long parsedPaymentId))
            {
                throw new ArgumentException("El paymentId no es válido.");
            }

            // Crear un cliente de pagos
            var client = new PaymentClient();

            // Obtener los detalles del pago utilizando el paymentId
            Payment payment = await client.GetAsync(parsedPaymentId);

            // Retornar el estado del pago (approved, pending, rejected, etc.)
            return payment.Status;
        }
     }
    
}
