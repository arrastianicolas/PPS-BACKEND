using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Application.Services
{
    public class MembershipCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMailService _mailService;

        public MembershipCheckService(IServiceProvider serviceProvider, IMailService mailService)
        {
            _serviceProvider = serviceProvider;
            _mailService = mailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

                    var clients = clientRepository.GetAllActiveClients();

                    foreach (var client in clients)
                    {
                        var minutesLefts = (client.Actualdatemembership.AddDays(30) - DateTime.Now).Days;

                        if (minutesLefts == 7 && client.Isactive == 1)
                        {
                            _mailService.Send(
                                $"Aviso: Su MEMBRESIA {client.Typememberships} del Training Center vencerá en 7 días",
                                $"Hola {client.Firstname}, su membresía {client.Typememberships} vencerá el {client.Actualdatemembership.AddDays(30)}.",
                                client.IduserNavigation.Email
                            );
                        }

                        if (minutesLefts <= 0 && client.Isactive == 1)
                        {
                            // Desactivar membresía
                            client.Isactive = 0;
                            clientRepository.Update(client);

                            // Enviar correo de expiración
                            _mailService.Send(
                                $"Aviso: SU MEMBRESIA {client.Typememberships} del Training Center HA EXPIRADO",
                                $"Hola {client.Firstname}, su membresía {client.Typememberships} ha expirado el {client.Actualdatemembership.AddDays(30)}.",
                                client.IduserNavigation.Email
                            );
                        }
                    }
                }

                // Esperar 24 horas antes de volver a ejecutar
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
