using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ShiftResetService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ShiftResetService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Revisar si es medianoche
                var currentTime = DateTime.Now;
                if (currentTime.Hour == 0 && currentTime.Minute == 0)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var shiftRepository = scope.ServiceProvider.GetRequiredService<IShiftRepository>();

                        // Obtener todos los turnos y restablecer los valores
                        var shifts = shiftRepository.Get();
                        foreach (var shift in shifts)
                        {
                            shift.Peoplelimit = 30;
                            shift.Actualpeople = 0;
                            shiftRepository.Update(shift);
                        }
                    }

                    // Esperar un minuto para evitar múltiples ejecuciones dentro de la misma medianoche
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }

                // Revisar cada minuto si es medianoche
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
