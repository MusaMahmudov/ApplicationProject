using AbilloLLCApplication.Core.Entities;
using AbilloLLCApplication.Core.Entities.Common;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace AbilloLLCApplication.Database.Contexts
{
    public class AppDbContext  : IdentityDbContext<AppUser>
    {
        private readonly IHttpContextAccessor _accessor;
        public AppDbContext(DbContextOptions<AppDbContext> options,IHttpContextAccessor accessor) : base(options) 
        {
            _accessor = accessor;


        }
      
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<Cargo> Cargoes { get; set; } = null!;
        public DbSet<Offer> Offers { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            string userName = "Admin";
            if (_accessor.HttpContext is not null)
            {
                userName = _accessor.HttpContext.User?.Identity?.Name;
            }
            if(userName is null || string.IsNullOrEmpty(userName))
            {
                userName = "Admin";
            }

            var entities = ChangeTracker.Entries<BaseSectionEntity>();
            foreach(var entity in entities)
            {
                switch(entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedAt = DateTime.UtcNow;
                        entity.Entity.CreatedBy = userName;
                        entity.Entity.UpdatedAt = DateTime.UtcNow;
                        entity.Entity.UpdatedBy = userName;
                        break;
                    case EntityState.Modified:
                        entity.Entity.UpdatedBy = userName;
                        entity.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Offer>().HasQueryFilter(o => o.IsDeleted == false);

            builder.ApplyConfigurationsFromAssembly(typeof(DriverConfiguration).Assembly);
            builder.Entity<Message>()
           .HasOne(m => m.Receiver)
           .WithMany(u => u.ReceiveMessages)
           .HasForeignKey(m => m.ReceiverId)
           .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
           .HasOne(m => m.Sender)
           .WithMany(u => u.SendedMessages)
         .HasForeignKey(m => m.SenderId)
         .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
