using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMercadoPagoService
    {
        Task<Preference> CrearPreferenciaPago(PreferenceRequest preferenceRequest);
        Task<Payment> ObtenerPagoPorId(string paymentId);
    }
}
