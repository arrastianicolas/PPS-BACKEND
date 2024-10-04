using Application.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MercadoPagoService : IMercadoPagoService
    {
        public MercadoPagoService()
        {
            
        }

        public async Task<Preference> CrearPreferenciaPago(PreferenceRequest preferenceRequest)
        {
            var client = new PreferenceClient();
            var preference = await client.CreateAsync(preferenceRequest);
            return preference;
        }
    }
}
