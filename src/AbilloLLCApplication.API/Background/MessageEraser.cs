using AbilloLLCApplication.Core.Entities;
using AbilloLLCApplication.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AbilloLLCApplication.API.Background
{
    public class MessageEraser : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public MessageEraser(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var cargoes = await dbContext.Cargoes.ToListAsync();

                    var cargoesToDelete = new List<Cargo>();

                    foreach (var cargo in cargoes)
                    {
                        if (cargo.CreatedAt < DateTime.UtcNow.AddHours(-1))
                        {
                            cargoesToDelete.Add(cargo);
                        }
                    }

                    dbContext.Cargoes.RemoveRange(cargoesToDelete);
                    await dbContext.SaveChangesAsync();


                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            }




        }
    }
}
