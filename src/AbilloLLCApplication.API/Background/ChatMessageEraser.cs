using AbilloLLCApplication.Database.Contexts;

namespace AbilloLLCApplication.API.Background
{
    public class ChatMessageEraser : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ChatMessageEraser(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                    var messages = dbContext?.Messages;
                    var messagesToDelete = messages.Where(m => m.CreatedAt < DateTime.UtcNow.AddDays(-7));
                    dbContext.RemoveRange(messagesToDelete);
                    await dbContext.SaveChangesAsync();


                }
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);


            }


        }
    }
}
}
