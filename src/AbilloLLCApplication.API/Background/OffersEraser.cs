using AbilloLLCApplication.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AbilloLLCApplication.API.Background
{
    public class OffersEraser : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public OffersEraser(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                        var offers = await dbContext.Offers.ToListAsync();
                        var offersToDelete = offers.Where(o => o.CreatedAt < DateTime.UtcNow.AddDays(-1));
                        if (offersToDelete.Count() > 0)
                        {
                            dbContext.RemoveRange(offersToDelete);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

            }

        }
    }
}
