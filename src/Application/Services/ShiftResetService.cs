using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
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
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Obtener la hora local ajustada a la zona horaria
                var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                Console.WriteLine($"Current Local Time: {currentTime}");

                if (currentTime.Hour == 14 && currentTime.Minute == 57)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var shiftRepository = scope.ServiceProvider.GetRequiredService<IShiftRepository>();
                        var shiftClientRepository = scope.ServiceProvider.GetRequiredService<IShiftClientRepository>();

                        // Obtener todos los turnos y restablecer los valores
                        var shifts = shiftRepository.Get();
                        foreach (var shift in shifts)
                        {
                            if (shift.Actualpeople != 0)
                            {

                                shift.Actualpeople = 0;
                                shiftRepository.Update(shift);

                            }


                        }

                        // Eliminar todos los registros de ShiftClient
                        shiftClientRepository.DeleteAll(); // Asegúrate de tener este método en tu repositorio
                    }

                    // Esperar un minuto para evitar múltiples ejecuciones dentro del mismo minuto
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }

                // Revisar cada minuto
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}